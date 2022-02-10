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
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = FadeCoroutine(time);
            StartCoroutine(fadeCoroutine);
        }

        private IEnumerator FadeCoroutine(float time)
        {
            flash = 0;
            float t = 0f;

            yield return new WaitForSeconds(1f);
            // Pour les spawn particules
            float previousT = 0f;
            SpriteRenderer spriteRender = spriteRenderer.GetComponent<SpriteRenderer>(); // à opti

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
            readableText.ReadPixels(new Rect((int)spriteRender.sprite.rect.x, (int)spriteRender.sprite.rect.y, (int)spriteRender.sprite.rect.width, (int)spriteRender.sprite.rect.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);

            // Pour que ça marche cette ligne il faut que la texture soit readable (donc ça casse les reins c'est dangereux)
            Color[] colors = readableText.GetPixels();// ((int)spriteRender.sprite.rect.x, (int)spriteRender.sprite.rect.y, (int)spriteRender.sprite.rect.width, (int)spriteRender.sprite.rect.height);

            while (t < time)
            {
                flash = Mathf.Lerp(0, 1, (t / time));
                t += Time.deltaTime;
                spriteRenderer.material.SetFloat("_Disappear", flash);
                SpawnParticle(spriteRender, colors, previousT, flash);
                yield return null;
                previousT = flash;
            }
            spriteRenderer.material.SetFloat("_Disappear", 1);

        }

        private void SpawnParticle(SpriteRenderer spriteRender, Color[] colors, float start, float end)
        {
            int startPixel = colors.Length-1 - (int) Mathf.Clamp(start * colors.Length, 0, colors.Length - 1);
            int endPixel = colors.Length-1 - (int) Mathf.Clamp(end * colors.Length, 0, colors.Length - 1);

            for (int i = startPixel; i >= endPixel; i--)
            {
                if (colors[i].a > 0)
                {
                    float x = i % spriteRender.sprite.rect.width;
                    float y = i / spriteRender.sprite.rect.width;

                    float offsetX = x / spriteRender.sprite.rect.width - (spriteRender.sprite.pivot.x / spriteRender.sprite.rect.width); 
                    float sizeX = spriteRender.transform.lossyScale.x * (spriteRender.sprite.rect.width / spriteRender.sprite.pixelsPerUnit);

                    float offsetY = y / spriteRender.sprite.rect.height - (spriteRender.sprite.pivot.y / spriteRender.sprite.rect.height);
            float   sizeY = spriteRender.transform.lossyScale.y * (spriteRender.sprite.rect.height / spriteRender.sprite.pixelsPerUnit);
                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = new Vector3(sizeX * offsetX, sizeY * offsetY, 0);
                    emitParams.startSize = spriteRender.transform.lossyScale.x / spriteRender.sprite.pixelsPerUnit;
                    emitParams.startColor = colors[i];
                    particle.Emit(emitParams, 1);
                }
            }
        }

        /*private void SpawnParticle()
        {
            SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
            Color[] colors = spriteRender.sprite.texture.GetPixels();
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].a > 0)
                {
                    float x = i % spriteRender.sprite.texture.width;
                    float y = i / spriteRender.sprite.texture.width;

                    float offsetX = x / spriteRender.sprite.texture.width - 0.5f; // Part du principeque le pivot est au centre
                    float sizeX = spriteRender.sprite.texture.width / spriteRender.sprite.pixelsPerUnit;

                    float offsetY = y / spriteRender.sprite.texture.height; // Part du principeque le pivot est au centre
                    float sizeY = spriteRender.sprite.texture.height / spriteRender.sprite.pixelsPerUnit;
                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = new Vector3(sizeX * offsetX, sizeY * offsetY, 0);
                    emitParams.startSize = 1 / spriteRender.sprite.pixelsPerUnit;
                    emitParams.startColor = colors[i];
                    particle.Emit(emitParams, 1);
                }
            }
        }*/

    }
}
