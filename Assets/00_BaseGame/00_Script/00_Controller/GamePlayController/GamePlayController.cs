
public class GamePlayController : Singleton<GamePlayController>
{
    public GameScene gameScene;
    public ItemSpotsManager itemSpotsManager;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_DontDestroyOnLoad = false;
        Init();
    }


    private void Init()
    {
        gameScene.Init();
        itemSpotsManager.Init();
    }
}
