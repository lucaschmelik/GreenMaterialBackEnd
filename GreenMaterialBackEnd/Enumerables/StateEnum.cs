namespace GreenMaterialBackEnd.Enumerables
{
    public enum StateEnum
    {
        None = 0,
        Created = 1,
        Confirmed = 2,
        NotPayed = 3,
        Payed = 4,
        Finished = 5,
        Canceled = 6
    }

    public static class StateExtensions
    {
        public static bool IsInProcess(int state)
        {
            return state is (int)StateEnum.Created or (int)StateEnum.Confirmed or (int)StateEnum.NotPayed;
        }
    }
}
