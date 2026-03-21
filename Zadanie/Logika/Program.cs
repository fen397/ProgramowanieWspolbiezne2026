using Logika;

double a;
double b;
double c;

Console.WriteLine("Podaj a:");
a = Convert.ToDouble(Console.ReadLine());
Console.WriteLine("Podaj b:");
b = Convert.ToDouble(Console.ReadLine());
Console.WriteLine("Podaj c:");
c = Convert.ToDouble(Console.ReadLine());

Delta delta = new Delta(a,b,c);

double wyn = delta.CalculateDelta();
Console.WriteLine("Delta wynosi: " + wyn);
if(wyn < 0)
{
    Console.WriteLine("Brak pierwiastków rzeczywistych.");
}
else if (wyn == 0)
{
    Console.WriteLine("Miejsce zerowe wynosi: " + delta.CalculateX1());
}
else
{
    Console.WriteLine("Pierwiastek 1 wynosi: " + delta.CalculateX1());
    Console.WriteLine("Pierwiastek 2 wynosi: " + delta.CalculateX2());
}


