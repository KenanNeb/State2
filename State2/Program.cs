namespace StateMachineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var fsm = new FiniteStateMachine();
            while (true)
            {
                if (fsm.State == FiniteStateMachine.States.EnterVoucherCode)
                {
                    Console.WriteLine("State: " + fsm.State);
                    Console.WriteLine("Enter Voucher Code:");
                    string voucherCode = Console.ReadLine();
                    Console.WriteLine("voucher is " + voucherCode);
                    Console.WriteLine();
                    fsm.ProcessEvent(FiniteStateMachine.Events.PressNext);
                }

                if (fsm.State == FiniteStateMachine.States.EnterTotalSale)
                {
                    Console.WriteLine("State: " + fsm.State);
                    Console.WriteLine("Enter Total Sale or x to simulate back");
                    string voucherSaleAmount = Console.ReadLine();
                    if (voucherSaleAmount == "x")
                        fsm.ProcessEvent(FiniteStateMachine.Events.PressBackToVoucherCode);
                    else
                    {
                        Console.WriteLine("total sale is " + voucherSaleAmount);
                        Console.WriteLine();
                        fsm.ProcessEvent(FiniteStateMachine.Events.PressRedeem);
                    }
                }

                if (fsm.State == FiniteStateMachine.States.ProcessVoucher)
                {
                    Console.WriteLine("State: " + fsm.State);
                    Console.WriteLine("Press 1 to fake a successful redeem:");
                    Console.WriteLine("Press 2 to fake a fail redeem:");
                    Console.WriteLine("Press 3 to do something stupid - press the Next Button which isn't allowed from this screen");
                    Console.WriteLine();
                    string result = Console.ReadLine();

                    //EnterVoucherCode stateidir
                    if (result == "1")
                        fsm.ProcessEvent(FiniteStateMachine.Events.ProcessSuccess);
                    if (result == "2")
                        fsm.ProcessEvent(FiniteStateMachine.Events.ProcessFail);
                    if (result == "3")
                        fsm.ProcessEvent(FiniteStateMachine.Events.PressNext);
                }

              
            }
        }
    }

    class FiniteStateMachine
    {
        
        public enum States { EnterVoucherCode, EnterTotalSale, ProcessVoucher };
        public enum Events { PressNext, PressRedeem, ProcessSuccess, ProcessFail, PressBackToVoucherCode };
        public delegate void ActionThing();

        public States State { get; set; }

        private ActionThing[,] fsm;

        public FiniteStateMachine()
        {
            
            fsm = new ActionThing[3, 5] {        
        {PressNext,null,       null,          null,       null},                  //EnterVoucherCode.... növbəti düyməsini sıxa bilər
        {null,     PressRedeem,null,          null,       PressBackToVoucherCode},//EnterTotalSale... Redeem və ya BackToVoucherCode düyməsini sıxa bilər
        {null,     null,       ProcessSuccess,ProcessFail,null} };                //ProcessVoucher -dən hərəkət edir... prosesSuccess və ya ProcessFail ola bilər. geri qaytarmaq üçün redeem edə bilməz
        }
        public void ProcessEvent(Events theEvent)
        {
            try
            {
                var row = (int)State;
                var column = (int)theEvent;
                
                fsm[row, column].Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        private void PressNext() { State = States.EnterTotalSale; }
        private void PressRedeem() { State = States.ProcessVoucher; }
        private void ProcessSuccess() { State = States.EnterVoucherCode; }
        private void ProcessFail() { State = States.EnterVoucherCode; }
        private void PressBackToVoucherCode() { State = States.EnterVoucherCode; }
    }
}