#pragma once
#include "Tessera.h"
#include "Posizione.h"

class Orario {
private:
    Tessera* celle[5][8];
    bool PosizioneValida(Posizione posizione);
public:
    Orario();
    ~Orario();

    // Non e' prevista la copia: l'orario possiede le tessere allocate con new.
    Orario(const Orario&) = delete;
    Orario& operator=(const Orario&) = delete;

    // Restituisce un puntatore NON proprietario: non chiamare delete sul risultato.
    Tessera* Get(Posizione posizione);
    // Se restituisce true, Orario acquisisce la proprieta' della tessera.
    // Accetta solo celle vuote e puntatori non nulli.
    bool Set(Posizione posizione, Tessera* tessera);
    void ScambiaCelle(Posizione prima, Posizione seconda);
};
