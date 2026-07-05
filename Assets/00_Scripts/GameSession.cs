using FootNailGame;

public static class GameSession
{
    // Kept as a plain public field: existing code (StageMapScreen, GameManager) writes/reads it directly.
    public static int SelectedStageIndex;

    public static WorldData SelectedWorld { get; private set; }
    public static StageGroupData SelectedStageGroup { get; private set; }
    public static CustomerStageData SelectedCustomer { get; private set; }
    public static NailDesignData SelectedNailDesign { get; private set; }
    public static StageReturnTarget ReturnTarget { get; private set; } = StageReturnTarget.StageMap;
    public static bool ReopenRepairShopLobby { get; private set; }

    public static void SetSelectedStageIndex(int stageIndex)
    {
        SelectedStageIndex = stageIndex;
    }

    public static void SetSelectedWorld(WorldData world)
    {
        SelectedWorld = world;
    }

    public static void SetSelectedStageGroup(StageGroupData stageGroup)
    {
        SelectedStageGroup = stageGroup;
    }

    public static void SetSelectedCustomer(CustomerStageData customer)
    {
        SelectedCustomer = customer;
    }

    public static void SetSelectedNailDesign(NailDesignData design)
    {
        SelectedNailDesign = design;
    }

    public static void PrepareStageReturn(StageReturnTarget target)
    {
        ReturnTarget = target;
        ReopenRepairShopLobby = target == StageReturnTarget.RepairShopLobby;
    }

    public static bool HasValidInGameSelection()
    {
        return SelectedStageGroup != null && SelectedCustomer != null;
    }

    public static void ClearCustomerSelection()
    {
        SelectedCustomer = null;
    }

    public static void ClearAll()
    {
        SelectedStageIndex = 0;
        SelectedWorld = null;
        SelectedStageGroup = null;
        SelectedCustomer = null;
        SelectedNailDesign = null;
        ReturnTarget = StageReturnTarget.StageMap;
        ReopenRepairShopLobby = false;
    }
}
