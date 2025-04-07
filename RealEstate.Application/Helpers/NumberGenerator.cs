

namespace RealEstate.Application.Helpers
{
    public class NumberGenerator
    {
        public static string CodeGenerator()
        {
            Random rand = new Random();
            string codigo = "";

            for (int i = 0; i < 4; i++)
            {
                codigo += rand.Next(0, 10).ToString();
            }
            return codigo;
        }
    }
}
