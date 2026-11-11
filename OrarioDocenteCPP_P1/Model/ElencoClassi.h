#pragma once
#include <string>
#include <vector>
#include "Classe.h"

// Archivio di classi: il vector possiede direttamente gli oggetti per valore.
class ElencoClassi {
private:
    std::vector<Classe> classi;
public:
    // Inserisce una copia se la sigla e' non vuota e non ancora presente.
    bool Aggiungi(const Classe& classe);
    // Restituisce l'indice o -1 se la sigla non e' presente.
    int Cerca(std::string sigla) const;
    int GetNumeroClassi() const;
    // Puntatore NON proprietario: puo' essere invalidato da Aggiungi.
    const Classe* Get(int indice) const;
};
