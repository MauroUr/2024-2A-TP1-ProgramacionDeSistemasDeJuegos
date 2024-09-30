using Audio;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemySfx : MonoBehaviour
    {
        [SerializeField] private RandomContainer<AudioClipData> spawnClips;
        [SerializeField] private RandomContainer<AudioClipData> explosionClips;
        private Enemy _enemy;

        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();
    
        private void FetchComponents()
        {
            // "a ??= b" is equivalent to "if(a == null) a = b" 
            _enemy ??= GetComponent<Enemy>();
        }
        
        private void OnEnable()
        {
            _enemy.OnSpawn += HandleSpawn;
            _enemy.health.OnDeath += HandleDeath;
        }
        
        private void OnDisable()
        {
            _enemy.OnSpawn -= HandleSpawn;
            _enemy.health.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            PlayRandomClip(explosionClips);
        }

        private void HandleSpawn()
        {
            PlayRandomClip(spawnClips);
        }

        private void PlayRandomClip(RandomContainer<AudioClipData> container)
        {
            if (!container.TryGetRandom(out var clipData))
                return;
            
            AudioManager.instance.Play(clipData, transform.position);
        }
    }
}
