#include "ElencoClassi.h"

bool ElencoClassi::Aggiungi(const Classe& classe) {
    if (classe.GetSigla().empty() || Cerca(classe.GetSigla()) != -1)
        return false;
    classi.push_back(classe); // Memorizza una copia dell'oggetto.
    return true;
}

int ElencoClassi::Cerca(std::string sigla) const {
    for (int indice = 0; indice < GetNumeroClassi(); indice++)
        if (classi[indice].GetSigla() == sigla) return indice;
    return -1;
}

int ElencoClassi::GetNumeroClassi() const {
    return static_cast<int>(classi.size());
}

const Classe* ElencoClassi::Get(int indice) const {
    if (indice < 0 || indice >= GetNumeroClassi()) return nullptr;
    return &classi[indice];
}
