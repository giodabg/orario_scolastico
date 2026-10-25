#pragma once

#include "Tessera.h"
#include "Posizione.h"

class Orario {
private:
    Tessera* celle[5][8];

public:
    Orario();
    ~Orario();

    Tessera* Get(Posizione posizione);
    bool Set(Posizione posizione, Tessera* tessera);
    void ScambiaCelle(Posizione prima, Posizione seconda);
};
