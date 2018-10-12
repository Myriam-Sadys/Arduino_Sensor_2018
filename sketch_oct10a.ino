int ThermistorPin = 0;
float adc;
float Rth, T;
int B = 3435;
float R0 = 10000;
float T0 = 298.15;

void setup() {
Serial.begin(9600);
}

void loop() {
  adc = analogRead(ThermistorPin);
  Serial.println(adc);
  Rth = R0*1024/adc - R0;
  Serial.println(Rth);
  T = 1/(log(Rth/R0)/B+1/T0) - 273.15;

  Serial.print("Temperature: ");
  Serial.print(T);
  Serial.println(" °C"); 

  delay(500);
}
