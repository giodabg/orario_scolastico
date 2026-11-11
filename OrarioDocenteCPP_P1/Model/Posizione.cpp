#include "Posizione.h"

Posizione::Posizione(int giorno, int ora) : giorno(giorno), ora(ora) {}
int Posizione::GetGiorno() { return giorno; }
int Posizione::GetOra() { return ora; }
