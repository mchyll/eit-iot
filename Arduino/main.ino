#include "TelenorNBIoT.h"      // https://github.com/ExploratoryEngineering/ArduinoNBIoT
#include <OneWire.h>           // https://github.com/PaulStoffregen/OneWire
#include <DallasTemperature.h> // https://github.com/milesburton/Arduino-Temperature-Control-Library

#define DEBUGGING 1
#define ONE_WIRE_BUS 4

/*
 * Program shell where the Arduino connects to the IoT-module was provided from Telenor at the library linked at the top of the file
 * and the rest of the program has been written with this as a base.
 */

// Magic for selecting serial port
#ifdef SERIAL_PORT_HARDWARE_OPEN
/*
 * For Arduino boards with a hardware serial port separate from USB serial.
 * This is usually mapped to Serial1. Check which pins are used for Serial1 on
 * the board you're using.
 */
#define ublox SERIAL_PORT_HARDWARE_OPEN
#else
/*
 * For Arduino boards with only one hardware serial port (like Arduino UNO). It
 * is mapped to USB, so we use SoftwareSerial on pin 10 and 11 instead.
 */
#include <SoftwareSerial.h>
SoftwareSerial ublox(10, 11);
#endif

// Configure mobile country code, mobile network code and access point name
// See https://www.mcc-mnc.com/ for country and network codes
// Access Point Namme: mda.ee (Telenor NB-IoT Developer Platform)
// Mobile Country Code: 242 (Norway)
// Mobile Network Operator: 01 (Telenor)
TelenorNBIoT nbiot("mda.ee", 242, 01);

IPAddress remoteIP(172, 16, 15, 14);
int REMOTE_PORT = 1234;

// Pins
int trigPin = 9;  // Trigger
int echoPin = 12; // Echo
int tempPin = 4;

int duration, distanceCm;

// Measurement variables for distance sensor
int distanceSum = 0;

// Measurement variables for distance sensor
int tempSum = 0;

// Define variables for the temp sensor
OneWire oneWire(ONE_WIRE_BUS); 
DallasTemperature sensors(&oneWire);

// Status and storage variables for loop
int temperature;
int distance;
int tempStatus;
int distanceStatus;
int counter = 0;
int samplesForMean = 10;

void nbiotInitialise() 
{
    if (DEBUGGING)
        Serial.println("Initialising NB-IoT");
    ublox.begin(9600);
    
    while (!nbiot.begin(ublox))
    {
        if (DEBUGGING)
            Serial.println("Begin failed. Retrying...");
        delay(1000);
    }

    if (DEBUGGING)
        Serial.println("Connecting to the web");
    while (!nbiot.online());

    if (DEBUGGING)
        Serial.println("Opening a socket");
    while (!nbiot.createSocket());

    if (DEBUGGING)
        Serial.println("NB-IoT initialised");
}

void setup()
{
    if (DEBUGGING)
        Serial.begin(9600);
        while (!Serial);
        Serial.println("Serial ready");

    if (DEBUGGING)
        Serial.println("Setting up the sensors");

    pinMode(trigPin, OUTPUT);
    pinMode(echoPin, INPUT);
    sensors.begin(); 

    nbiotInitialise();

    if (DEBUGGING)
        Serial.println("Initialised");
}


void loop()
{
    // Reading ultrasonic sensor
    digitalWrite(trigPin, LOW);
    delayMicroseconds(5);
    digitalWrite(trigPin, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigPin, LOW);
    pinMode(echoPin, INPUT);
    duration = pulseIn(echoPin, HIGH);

    // Computing distance from the pules duration
    distanceCm = (duration / 2) / 29.1;
    distanceSum += distanceCm;

    // Reading temperature
    sensors.requestTemperatures(); 
    temperature = sensors.getTempCByIndex(0);
    tempSum += temperature;

    // Incrementing the counter
    counter++;

    // Send data if we have sampled ten samples
    if (counter == samplesForMean) 
    {
        distance = distanceSum / samplesForMean;
        temperature = tempSum / samplesForMean;

        if (DEBUGGING) 
            Serial.println("Payload sent " + String(distanceSum) +  " " + String(tempSum));
        nbiot.sendString(remoteIP, REMOTE_PORT, String(temperature) + " " + String(distance));  

        // Resetting
        distanceSum = 0;
        tempSum = 0;
        counter = 0;
    }

    delay(1000);
}