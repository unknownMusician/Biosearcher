using System;
using System.Threading;
using System.Threading.Tasks;
using AreYouFruits.Common;
using UnityEngine;

namespace Biosearcher.Plants
{
    public class Plant : IDisposable
    {
        private const string GrowthAnimationName = "Growth";

        private readonly Animator _animator;
        public event Action? OnGrowEnd;

        private bool _isGrowing = false;
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        
        public PlantInfo PlantInfo { get; private set; }

        public Plant(Animator animator)
        {
            _animator = animator;
        }
        
        // todo
        public void TryStartGrow(PlantInfo plantInfo)
        {
            if (!_isGrowing)
            {
                PlantInfo = plantInfo;
                GrowAsync();
            }
        }

        private async Task GrowAsync()
        {
            _isGrowing = true;
            //_animation.Play();

            await Tasks.LerpAsync(PlantInfo.GrowthTime, HandleGrowth, cancellation: _cancellationSource.Token);
            
            _isGrowing = false;

            OnGrowEnd?.Invoke();
        }

        private void HandleGrowth(float lerp) => _animator.Play(GrowthAnimationName, -1, lerp);

        public void Dispose() => _cancellationSource.Cancel();
    }
}
