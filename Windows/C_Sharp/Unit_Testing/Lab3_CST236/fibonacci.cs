using System;

public class Class1
{
        public int Fibonacci(int Factor)
		{
            int x,y;

            if (Factor < 2)
            {
                return Factor;
            }
            x = Fibonacci(--Factor);
            y = Fibonacci(--Factor);
			
			return (x+y);
		}

}
