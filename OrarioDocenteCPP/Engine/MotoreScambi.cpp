#include "MotoreScambi.h"

bool MotoreScambi::Scambia(Orario& orario, Posizione prima, Posizione seconda) {
    if (prima.GetGiorno() < 0 || prima.GetGiorno() >= 5 ||
        prima.GetOra() < 0 || prima.GetOra() >= 8 ||
        seconda.GetGiorno() < 0 || seconda.GetGiorno() >= 5 ||
        seconda.GetOra() < 0 || seconda.GetOra() >= 8) return false;

    if (prima.GetGiorno() == seconda.GetGiorno() &&
        prima.GetOra() == seconda.GetOra()) return false;

    // L'orario di partenza deve essere privo di buchi.
    if (!validatore.SenzaBuchi(orario)) return false;

    orario.ScambiaCelle(prima, seconda);
    if (validatore.SenzaBuchi(orario)) return true;

    // Se lo scambio crea un buco, ripristina l'orario precedente.
    orario.ScambiaCelle(prima, seconda);
    return false;
}
