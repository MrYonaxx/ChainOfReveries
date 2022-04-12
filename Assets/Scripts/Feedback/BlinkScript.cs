using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing {
    public class BlinkScript : MonoBehaviour
    {
        float flash = 0;
        [SerializeField]
        Renderer spriteRenderer;
        [SerializeField]
        ParticleSystem particle = null;
        private IEnumerator blinkCoroutine;
        private IEnumerator fadeCoroutine;


        //[SerializeField]
        //Sprite spriteDeath = null;

        [SerializeField]
        bool debug = false;
        private void Start()
        {
            if (debug)
                Fade(10);
        }

        public void Blink(float time, Color blinkColor)
        {
            if (!isActiveAndEnabled)
                return;
            if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);
            blinkCoroutine = BlinkCoroutine(time, blinkColor);
            StartCoroutine(blinkCoroutine);
        }

        private IEnumerator BlinkCoroutine(float time, Color blinkColor)
        {
            flash = 1;
            float t = 0f;
            spriteRenderer.material.SetColor("_Color", blinkColor);
            while (t < time)
            {
                flash = Mathf.Lerp(1, 0, (t / time));
                t += Time.deltaTime;
                spriteRenderer.material.SetFloat("_FlashAmount", flash);
                yield return null;
            }
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
        }


        // à bouger mais en meme temps c'est le même shader
        public void Fade(float time)
        {
            if (!isActiveAndEnabled)
                return;

           /* if (blinkCoroutine != null)
                StopCoroutine(blinkCoroutine);
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
           */
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = FadeCoroutine(time);
            StartCoroutine(fadeCoroutine);
        }

        private IEnumerator FadeCoroutine(float time)
        {
            float fade = 0;
            float t = 0f;

            // Pour les spawn particules
            float previousT = 0f;
            SpriteRenderer spriteRender = spriteRenderer.GetComponent<SpriteRenderer>(); // à optirr

            RenderTexture renderTex = RenderTexture.GetTemporary(
                 (int)spriteRender.sprite.texture.width,
                 (int)spriteRender.sprite.texture.height,
                 0,
                 RenderTextureFormat.Default,
                 RenderTextureReadWrite.Linear);

            Graphics.Blit(spriteRender.sprite.texture, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D((int)spriteRender.sprite.rect.width, (int)spriteRender.sprite.rect.height);
            readableText.ReadPixels(new Rect((int)spriteRender.sprite.rect.x, (int)spriteRender.sprite.texture.height - spriteRender.sprite.rect.y - spriteRender.sprite.rect.height, (int)spriteRender.sprite.rect.width, (int)spriteRender.sprite.rect.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            // On set ces paramètres pour bien gérer la disparition du shader en utilisant la height de l'atlas et pas du spritesheet en entier
            // y = 0 c'est en bas de la texture et pas en haut et on fait disparaitre le sprite à l'envers
            spriteRenderer.material.SetFloat("_AtlasStartHeight", (spriteRender.sprite.texture.height - spriteRender.sprite.rect.yMax) / spriteRender.sprite.texture.height);
            spriteRenderer.material.SetFloat("_AtlasEndHeight", (spriteRender.sprite.texture.height - spriteRender.sprite.rect.y) / spriteRender.sprite.texture.height);

            Color[] colors = readableText.GetPixels();

            while (t < time)
            {
                fade = Mathf.Lerp(0, 1, (t / time));
                t += Time.deltaTime;
                spriteRenderer.material.SetFloat("_Disappear", fade);
                SpawnParticle(spriteRender, colors, previousT, fade);
                yield return null;
                previousT = fade;
            }
            spriteRenderer.material.SetFloat("_Disappear", 1);

        }

        private void SpawnParticle(SpriteRenderer spriteRender, Color[] colors, float start, float end)
        {
            // On récupère tout les pixels de la ligne qui nous intéresse
            int startPixel = colors.Length-1 - (int) Mathf.Clamp(start * colors.Length, 0, colors.Length - 1);
            int endPixel = colors.Length-1 - (int) Mathf.Clamp(end * colors.Length, 0, colors.Length - 1);

            for (int i = startPixel; i >= endPixel; i--)
            {
                if (colors[i].a > 0)
                {
                    // On spawn ligne par ligne
                    float x = i % spriteRender.sprite.rect.width;
                    float y = i / spriteRender.sprite.rect.width;

                    // On calcul l'offset en X et on ajuste en fonction de la taille du sprite, et donc de la taille des pixels
                    // Le +0.5 c'est pour prendre en compte un demi pixel, vu que le pivot d'une particle est au centre du pixel
                    float offsetX = (x+0.5f) / spriteRender.sprite.rect.width - (spriteRender.sprite.pivot.x / spriteRender.sprite.rect.width);

                    float sizeX = spriteRender.transform.lossyScale.x * (spriteRender.sprite.rect.width / spriteRender.sprite.pixelsPerUnit);

                    float offsetY = (y - 0.5f) / spriteRender.sprite.rect.height - (spriteRender.sprite.pivot.y / spriteRender.sprite.rect.height);
                    float sizeY = spriteRender.transform.lossyScale.y * (spriteRender.sprite.rect.height / spriteRender.sprite.pixelsPerUnit);

                    // Emission de particle
                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = new Vector3(sizeX * offsetX, sizeY * offsetY, 0);
                    emitParams.startSize = spriteRender.transform.lossyScale.x / spriteRender.sprite.pixelsPerUnit;
                    emitParams.startColor = colors[i];
                    particle.Emit(emitParams, 1);
                }
            }
        }


    }
}
