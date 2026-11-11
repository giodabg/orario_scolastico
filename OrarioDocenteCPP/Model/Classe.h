#pragma once
#include <string>

// Descrive una classe scolastica: un oggetto Classe viene copiato nel vector.
class Classe {
private:
    std::string sigla;
    std::string indirizzo;
    int numeroStudenti;
public:
    Classe(std::string sigla, std::string indirizzo, int numeroStudenti);
    std::string GetSigla() const;
    std::string GetIndirizzo() const;
    int GetNumeroStudenti() const;
};
