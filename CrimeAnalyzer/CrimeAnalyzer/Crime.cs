namespace CrimeAnalyzer
{
    public class Crime
    {


        public int year;
        public int population;
        public int violent;
        public int murder;
        public int rape;
        public int robbery;
        public int assault;
        public int property;
        public int burglary;
        public int theft;
        public int motor; 

        public Crime(int Year, int Population, int Violent, int Murder, int Rape, int Robbery, int Assault, int Property, int Burglary, int Theft, int Motor)
        {
            year = Year;
            population = Population;
            violent = Violent;
            murder = Murder;
            rape = Rape;
            robbery = Robbery;
            assault = Assault;
            property = Property;
            burglary = Burglary;
            theft = Theft;
            motor = Motor;
        }   
    }
}
