#include <Wire.h>
#include <Uduino.h>

Uduino accelero("boardOne");

// Broche d'Arduino pour communication I2C avec accéléromètre LG-ADXL345.
const int ACCELEROMETRE_1_SCL = 21;
const int ACCELEROMETRE_1_SDA = 20;

//*****************************************************************************
// Déclaration des variables globales.
//*****************************************************************************

// Adresses de communication I2C avec accéléromètre ADXL345.
const char Accellerometre3AxesAdresse = 0x53;
byte Accellerometre3AxesMemoire [6];
const char POWER_CTL = 0x2D;// Registre "Power Control".
const char DATA_FORMAT = 0x31;// Registre "Data Format".
const char DATAX0 = 0x32;//X-Axis Data 0
const char DATAX1 = 0x33;//X-Axis Data 1
const char DATAY0 = 0x34;//Y-Axis Data 0
const char DATAY1 = 0x35;//Y-Axis Data 1
const char DATAZ0 = 0x36;//Z-Axis Data 0
const char DATAZ1 = 0x37;//Z-Axis Data 1

const char Accellerometre_1_Precision2G  = 0x00;
const char Accellerometre_1_Precision4G  = 0x01;
const char Accellerometre_1_Precision8G  = 0x02;
const char Accellerometre_1_Precision16G = 0x03;
const char Accellerometre_1_ModeMesure   = 0x08;

// Pour recevoir les valeurs des 3 axes de l'accéléromètre.
int Accelerometre1_AxeX = 0;
int Accelerometre1_AxeY = 0;
int Accelerometre1_AxeZ = 0;

const int N = 500, dT = 200;
const double toAcceleration = 3.9*9.81/1000, tolerance = 0.6;

int i = 0, j = 0, dirach = 0, hits = 0;
double s = 0, magAcc;
bool countHits = false;

//*****************************************************************************


//*****************************************************************************
// FONCTION SETUP = Code d'initialisation.
//*****************************************************************************

void setup () {
    Serial.begin(38400);
  
    // Initialisation de la communication I2C bus pour le capteur d’accélération.
    Wire.begin();
    // Mettre le ADXL345 à plage +/-2G en écrivant la valeur 0x01 dans le
    // registre DATA_FORMAT.
    AccellerometreConfigure(DATA_FORMAT, Accellerometre_1_Precision2G);
    // Mettre le ADXL345 en mode de mesure en écrivant 0x08 dans le registre
    // POWER_CTL.
    AccellerometreConfigure(POWER_CTL, Accellerometre_1_ModeMesure);

    accelero.addCommand("Calibrate", Calibrate);
    accelero.addCommand("CountHits", CountHits);
    //Calibrate();
}

void Calibrate() {
    Serial.println("Calibrating Accelerometer");
    countHits = false;
    s=0;
    for (int i=0; i<N; i++) {
        readAccelemrometer();
        s += magAcc;
        delay(10);
    }
    s /= N;
    Serial.println("Accelerometer Calibrated");
}

void CountHits() {
    Serial.println(accelero.getNumberOfParameters());
    int boolInt = false;
    delay(10);
    if (accelero.getNumberOfParameters() > 0) {
        boolInt = accelero.charToInt(accelero.getParameter(0));
    }
    if (boolInt) {
        countHits = true;
        Serial.println("Started counting hits");
    } else {
        countHits = false;
        Serial.println("Stopped counting hits");
    }
}

void readAccelemrometer() {
    AccellerometreLecture();
    magAcc = Accelerometre1_AxeX*toAcceleration + Accelerometre1_AxeY*toAcceleration + Accelerometre1_AxeZ*toAcceleration;
}

void loop() {
   accelero.update();

    if (accelero.isConnected()) {
        readAccelemrometer();
        
        if (countHits) {
            Serial.print(magAcc); Serial.print(" "); Serial.println(hits);
            if ((magAcc - s > tolerance || magAcc - s < -tolerance) && j > dT) {
                dirach = 1;
                hits++;
                j = 0;
            } else {
                dirach = 0;
            }
            j++;
        } else {
          Serial.print(magAcc); Serial.print(" "); Serial.println(hits);
        }
    }
    
    //readAccelemrometer();
    //Serial.println(magAcc);
    
    // Pour affichage dans le moniteur série de l'éditeur Arduino.
}

//*****************************************************************************
// FONCTION AccellerometreConfigure
//*****************************************************************************
void AccellerometreConfigure (byte address, byte val) {
    // Commencer la transmission à trois axes accéléromètre
    Wire.beginTransmission (Accellerometre3AxesAdresse);
    // Envoyer l'adresse de registre
    Wire.write (address);
    // Envoyer la valeur à écrire.
    Wire.write (val);
    // Fin de la transmission.
    Wire.endTransmission ();
}

//*****************************************************************************
// FONCTION AccellerometreLecture ()
//*****************************************************************************
void AccellerometreLecture () {
    uint8_t NombreOctets_a_Lire = 6;
    // Lire les données d'accélération à partir du module ADXL345.
    AccellerometreLectureMemoire (DATAX0, NombreOctets_a_Lire,
                                  Accellerometre3AxesMemoire);
  
    // Chaque lecture de l'axe vient dans une résolution de 10 bits, soit 2 octets.
    // Première Octet significatif !
    // Donc nous convertissons les deux octets pour un « int ».
    Accelerometre1_AxeX = (((int)Accellerometre3AxesMemoire[1]) << 8) |
        Accellerometre3AxesMemoire[0];
    Accelerometre1_AxeY = (((int)Accellerometre3AxesMemoire[3]) << 8) |
        Accellerometre3AxesMemoire[2];
    Accelerometre1_AxeZ = (((int)Accellerometre3AxesMemoire[5]) << 8) |
        Accellerometre3AxesMemoire[4];
}

//*****************************************************************************
// FONCTION AccellerometreLectureMemoire
//*****************************************************************************
void AccellerometreLectureMemoire (byte address, int num, byte
                                   Accellerometre3AxesMemoire[]) {
    // Démarrer la transmission à accéléromètre.
    Wire.beginTransmission (Accellerometre3AxesAdresse);
    // Envoie l'adresse de lire.
    Wire.write (address);
    // Fin de la transmission.
    Wire.endTransmission ();
    // Démarrer la transmission à accéléromètre.
    Wire.beginTransmission (Accellerometre3AxesAdresse);
    // Demande 6 octets à l'accéléromètre.
    Wire.requestFrom (Accellerometre3AxesAdresse, num);
  
    int i = 0;
    // L'accéléromètre peut envoyer moins que demandé, c'est anormal, mais...
    while (Wire.available())
    {
      // Recevoir un octet.
      Accellerometre3AxesMemoire[i] = Wire.read ();
      i++;
    }
      // Fin de la transmission.
    Wire.endTransmission ();
}
