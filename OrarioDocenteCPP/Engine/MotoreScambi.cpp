#include "MotoreScambi.h"

bool MotoreScambi::Scambia(Orario& orario, Posizione prima, Posizione seconda) {
    if (prima.GetGiorno() < 0 || prima.GetGiorno() >= 5 ||
        prima.GetOra() < 0 || prima.GetOra() >= 8 ||
        seconda.GetGiorno() < 0 || seconda.GetGiorno() >= 5 ||
        seconda.GetOra() < 0 || seconda.GetOra() >= 8) return false;

    if (prima.GetGiorno() == seconda.GetGiorno() &&
        prima.GetOra() == seconda.GetOra()) return false;

    // Il prototipo richiede un orario inizialmente valido.
    if (!validatore.ControlloOre(orario)) return false;

    orario.ScambiaCelle(prima, seconda);
    if (validatore.ControlloOre(orario)) return true;

    // La seconda chiamata ripristina esattamente lo stato precedente.
    orario.ScambiaCelle(prima, seconda);
    return false;
}
