int ThermistorPin = A0;
float adc;
float Rth, T;
int B = 3435;
//float R0 = 10000;
//float T0 = 298.15;
unsigned long TempAdc[][2]=
{
  50,0,
  150, 1677,
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
  unsigned long test_adc;
  test_adc = analogRead(ThermistorPin);
  Serial.print("test_adc : ");
  Serial.println(test_adc);
  SecondMethod(test_adc);
}

void SecondMethod(unsigned long test_adc)
{
  int i = 0;
  unsigned long TempA0, TempA1, TempT0, TempT1, TempTx;
  //while (TempAdc[i][0] <= test_adc)
  //{
    //i++;
  //}
  for (i = 1; test_adc > TempAdc[i][0];i++);
  TempA0 = TempAdc[i - 1][0];
  Serial.print("TempA0 : ");
  Serial.println(TempA0);
  TempA1 = TempAdc[i][0];
  Serial.print("TempA1 : ");
  Serial.println(TempA1);
  TempT0 = TempAdc[i - 1][1];
  Serial.print("TempT0 : ");
  Serial.println(TempT0);
  TempT1 = TempAdc[i][1];
  Serial.print("TempT1 : ");
  Serial.println(TempT1);
  //TempTx = TempT0 + ((test_adc - TempA0) * (TempT1 - TempT0)) / (TempA1 - TempA0);
  TempTx = map(test_adc, TempA0, TempA1, TempT0, TempT1);
  Serial.println((TempTx - 4000) / 100);
  delay(2000);
}

void FirstMethod()
{
  Rth = R0*1024/adc - R0;
  T = 1/(log(Rth/R0)/B+1/T0) - 273.15;

  Serial.print("Temperature: ");
  Serial.print(T);
  Serial.println(" °C"); 

  delay(500);
}
