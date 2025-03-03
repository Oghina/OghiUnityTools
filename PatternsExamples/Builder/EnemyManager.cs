using System.Collections.Generic;
using OghiUnityTools.DependencyInjection;
using UnityEngine;

namespace OghiUnityTools.PatternsExamples.Builder
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> enemies;

        private void Start()
        {
            enemies.ForEach(e => EnemyFactory.CreateEnemy(e));
        }
    }
}