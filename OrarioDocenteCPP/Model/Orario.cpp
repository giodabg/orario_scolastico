#include "Orario.h"

bool Orario::PosizioneValida(Posizione posizione) {
    return posizione.GetGiorno() >= 0 && posizione.GetGiorno() < 5 &&
           posizione.GetOra() >= 0 && posizione.GetOra() < 8;
}

Orario::Orario() {
    for (int giorno = 0; giorno < 5; giorno++)
        for (int ora = 0; ora < 8; ora++)
            celle[giorno][ora] = nullptr;
}

Orario::~Orario() {
    for (int giorno = 0; giorno < 5; giorno++)
        for (int ora = 0; ora < 8; ora++)
            delete celle[giorno][ora];
}

Tessera* Orario::Get(Posizione posizione) {
    if (!PosizioneValida(posizione)) return nullptr;
    return celle[posizione.GetGiorno()][posizione.GetOra()];
}

bool Orario::Set(Posizione posizione, Tessera* tessera) {
    if (!PosizioneValida(posizione) || tessera == nullptr) return false;
    if (celle[posizione.GetGiorno()][posizione.GetOra()] != nullptr) return false;
    // Evita di acquisire due volte lo stesso puntatore in celle diverse.
    for (int giorno = 0; giorno < 5; giorno++)
        for (int ora = 0; ora < 8; ora++)
            if (celle[giorno][ora] == tessera) return false;
    celle[posizione.GetGiorno()][posizione.GetOra()] = tessera;
    return true;
}

void Orario::ScambiaCelle(Posizione prima, Posizione seconda) {
    if (!PosizioneValida(prima) || !PosizioneValida(seconda)) return;
    Tessera* temporanea = celle[prima.GetGiorno()][prima.GetOra()];
    celle[prima.GetGiorno()][prima.GetOra()] = celle[seconda.GetGiorno()][seconda.GetOra()];
    celle[seconda.GetGiorno()][seconda.GetOra()] = temporanea;
}
