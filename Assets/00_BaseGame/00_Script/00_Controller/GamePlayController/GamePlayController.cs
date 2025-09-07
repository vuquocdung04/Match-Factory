
using Sirenix.OdinInspector;

public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    public ItemSpotsManager itemSpotsManager;
    public MergeManager mergeManager;
    public LevelManager levelManager;
    public GoalManager goalManager;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
        Init();
    }


    private void Init()
    {
        itemSpotsManager.Init();
        mergeManager.Init();
        levelManager.Init();
        goalManager.Init();
        gameScene.Init();
    }
}
