using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioClip))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance { get; private set; }
        private ObjectPool<AudioSource> audioSourcePool;

        [SerializeField] private GameObject audioPlayer;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            audioSourcePool = new ObjectPool<AudioSource>();
        }

        private void OnDisable()
        {
            //audioSourcePool.Clear();
        }

        public void Play(AudioClipData data, Vector3 position)
        { 
            var clipLength = data.Clip.length;

            AudioSource source = audioSourcePool.FindInactive();

            if (source == null)
            {
                source = Instantiate(audioPlayer, position, Quaternion.identity).GetComponent<AudioSource>();
                audioSourcePool.Register(source);
            }
            else
            {
                audioSourcePool.MarkAsActive(source);
                gameObject.transform.position = position;
                gameObject.SetActive(true);
            }

            source.loop = data.Loop;
            source.clip = data.Clip;
            source.outputAudioMixerGroup = data.Group;
            source.Play();

            StartCoroutine(DeactivateIn(clipLength, source));
        }
        
        private IEnumerator DeactivateIn(float seconds, AudioSource source)
        {
            yield return new WaitForSeconds(Mathf.Max(seconds, 0));
            audioSourcePool.MarkAsInactive(source);
            gameObject.SetActive(false);

        }
    }
}
