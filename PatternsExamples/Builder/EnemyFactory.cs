namespace OghiUnityTools.PatternsExamples.Builder
{
    public static class EnemyFactory 
    {
        public static Enemy CreateEnemy(EnemyData data)
        {
            return new Enemy.Builder()
                .WithName(data.name)
                .WithWeaponStrategy(data.WeaponStrategy)
                .WithDetectionStrategy(data.DetectionStrategy)
                .Build();
        }
    }
}