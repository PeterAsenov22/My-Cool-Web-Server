namespace WebServer.ByTheCakeApplication.Models
{
    using System;

    public class Calculator
    {
        public static decimal Calculate(decimal firstNumber, decimal secondNumber, string sign)
        {
            decimal result;

            switch (sign)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "/":
                    result = firstNumber / secondNumber;
                    break;
                default:
                    throw new InvalidOperationException("Invalid mathematical sign.");
            }

            return result;
        }
    }
}
