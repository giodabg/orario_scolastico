#include "Tessera.h"

Tessera::Tessera(std::string materia, std::string classe, std::string aula)
    : materia(materia), classe(classe), aula(aula) {}

std::string Tessera::GetMateria() { return materia; }
std::string Tessera::GetClasse() { return classe; }
std::string Tessera::GetAula() { return aula; }
