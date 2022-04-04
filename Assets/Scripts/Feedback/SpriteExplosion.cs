using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceActing 
{
    // En espèrant que c'est pas ultra gourmant 
    public class SpriteExplosion : MonoBehaviour
    {
        [SerializeField]
        SpriteRenderer spriteRenderer = null;

        [SerializeField]
        ParticleSystem particle = null;


        [SerializeField]
        float timeBeforeExplosion = 0.4f;
        [SerializeField]
        bool noColor = false;

        private void OnEnable()
        {
            StartCoroutine(ParticleExplosion(timeBeforeExplosion));
        }




        private IEnumerator ParticleExplosion(float time)
        {
            yield return new WaitForSeconds(time);
            // Copie le sprite dans une texture, qu'on vient ensuite lire
            RenderTexture renderTex = RenderTexture.GetTemporary(
                 (int)spriteRenderer.sprite.texture.width,
                 (int)spriteRenderer.sprite.texture.height,
                 0,
                 RenderTextureFormat.Default,
                 RenderTextureReadWrite.Linear);

            Graphics.Blit(spriteRenderer.sprite.texture, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D((int)spriteRenderer.sprite.rect.width, (int)spriteRenderer.sprite.rect.height);
            readableText.ReadPixels(new Rect((int)spriteRenderer.sprite.rect.x, (int)spriteRenderer.sprite.texture.height - spriteRenderer.sprite.rect.y - spriteRenderer.sprite.rect.height, (int)spriteRenderer.sprite.rect.width, (int)spriteRenderer.sprite.rect.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);


            Color32[] colors = readableText.GetPixels32();
            // Et on spawn
            SpawnParticle(spriteRenderer, colors);
            spriteRenderer.enabled = false;
        }

        private void SpawnParticle(SpriteRenderer spriteRender, Color32[] colors)
        {
            float scaleX = spriteRenderer.transform.lossyScale.x;/*spriteRender.transform.lossyScale.x*/
            float scaleY = spriteRenderer.transform.lossyScale.y;

            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].a > 0)
                {
                    // On spawn ligne par ligne
                    float x = i % spriteRender.sprite.rect.width;
                    float y = i / spriteRender.sprite.rect.width;

                    // On calcul l'offset en X et on ajuste en fonction de la taille du sprite, et donc de la taille des pixels
                    // Le +0.5 c'est pour prendre en compte un demi pixel, vu que le pivot d'une particle est au centre du pixel
                    float offsetX = (x+0.5f) / spriteRender.sprite.rect.width - (spriteRender.sprite.pivot.x / spriteRender.sprite.rect.width);

                    float sizeX = scaleX * (spriteRender.sprite.rect.width / spriteRender.sprite.pixelsPerUnit);

                    float offsetY = (y - 0.5f) / spriteRender.sprite.rect.height - (spriteRender.sprite.pivot.y / spriteRender.sprite.rect.height);
                    float sizeY = scaleY * (spriteRender.sprite.rect.height / spriteRender.sprite.pixelsPerUnit);

                    // Emission de particle
                    ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
                    emitParams.position = new Vector3(sizeX * offsetX, sizeY * offsetY, 0);
                    emitParams.startSize = spriteRender.transform.lossyScale.x / spriteRender.sprite.pixelsPerUnit;
                    if(!noColor) // So pas opti
                        emitParams.startColor = colors[i];
                    particle.Emit(emitParams, 1);
                }
            }
        }


    }
}
