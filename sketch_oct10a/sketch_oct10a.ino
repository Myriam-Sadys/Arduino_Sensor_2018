int ThermistorPin = A0;
int adc;
float Rth, T;
int B = 3435;
float R0 = 10000;
float T0 = 298.15;
unsigned long TempAdc[][2]=
{
  {50,0},
  {100, 390},
  {150, 1677},
  200, 2544,
  250, 3237,
  300, 3837,
  350, 4382,
  400, 4895,
  450, 5392,
  500, 5883,
  550, 6379,
  600, 6890,
  650, 7426,
  700, 8002,
  750, 8636,
  800, 9355,
  850, 10203,
  900, 11260,
  950, 12695,
  971, 14985,
  1023, 16567,
};
// get value from analogRead, find it between the first numbers then use calcul of class
// Tx = T0 + ((Ax - A0)(T1 - T0)) / (A1 - A0)
// tempx = map(Adcx, prevAdcValue0, postAdcValue1, prevAdvValue1, prevTemp0, postTemp1)

void setup() {
Serial.begin(9600);
}

void loop() {
  adc = analogRead(ThermistorPin);
  //Serial.print("test_adc : ");
  //Serial.println(test_adc);
  SecondMethod(adc);
}

void SecondMethod(int adc)
{
  int i;
  int TempA0, TempA1, TempT0, TempT1, TempTx;
  for (i = 1; adc > TempAdc[i][0];i++);
  TempA0 = TempAdc[i - 1][0];
  TempA1 = TempAdc[i][0];
  TempT0 = TempAdc[i - 1][1];
  TempT1 = TempAdc[i][1];
  TempTx = map(adc, TempA0, TempA1, TempT0, TempT1);
  Serial.print((TempTx - 4000)/100);
  Serial.print(".");
  Serial.println((TempTx - 4000)%100);
  delay(1000);
}

void FirstMethod()
{
  Rth = R0*1024/adc - R0;
  T = 1/(log(Rth/R0)/B+1/T0) - 273.15;
  //T = 0;

  //Serial.print("Temperature: ");
  Serial.println(T);
  //Serial.println(" °C"); 

  delay(500);
}
