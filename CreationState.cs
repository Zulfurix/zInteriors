namespace zInteriors_Client
{
    static class CreationState
    {
        public enum State
        {
            NOT_CREATING_INTERIOR = 0,
            PLACING_ENTRANCE = 1,
            PLACING_EXIT = 2
        }

        private static State playerCreationState = State.NOT_CREATING_INTERIOR;

        public static State PlayerCreationState
        {
            get { return playerCreationState; }
            set { playerCreationState = value; }
        }
    }
}
