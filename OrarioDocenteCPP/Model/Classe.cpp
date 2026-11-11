#include "Classe.h"

Classe::Classe(std::string sigla, std::string indirizzo, int numeroStudenti)
    : sigla(sigla), indirizzo(indirizzo), numeroStudenti(numeroStudenti) {}

std::string Classe::GetSigla() const { return sigla; }
std::string Classe::GetIndirizzo() const { return indirizzo; }
int Classe::GetNumeroStudenti() const { return numeroStudenti; }
