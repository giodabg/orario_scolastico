#pragma once

class Posizione {
private:
    int giorno;
    int ora;

public:
    Posizione(int giorno, int ora);

    int GetGiorno();
    int GetOra();
};
