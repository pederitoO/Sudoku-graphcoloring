Bienvenue sur le dépôt du TP Sudoku.....

# Présentation des solvers

Dans ce projet, plusieurs solveurs de Sudoku ont été implémentés, chacun utilisant une approche algorithmique différente pour résoudre la grille. Les solveurs exploitent les concepts de coloration de graphe, un domaine des mathématiques discrètes qui permet d'affecter des couleurs à des nœuds sous certaines contraintes.

## GraphColoringSolver
#### Modélise le Sudoku comme un problème de coloration de graphe :
- Chaque cellule correspond à un sommet (81 au total).
- Les contraintes (ligne, colonne, bloc) sont représentées par des arêtes.
- Les chiffres de 1 à 9 servent de couleurs.
#### Utilise un backtracking simple :
- Assigne une couleur à chaque sommet successivement.
- Vérifie la validité de la couleur attribuée.
- Revient en arrière si aucune couleur valide n’est disponible.

## VertexColoringSolver
#### Applique une stratégie plus efficace en intégrant des heuristiques :
- Sélectionne en priorité la cellule la plus contrainte (FindMostConstrainedVertex).
- Trie les couleurs possibles en fonction de leur fréquence (GetSortedAvailableColors).
- Utilise le backtracking en minimisant les retours en arrière.
#### Avantages :
- Commence par les cellules les plus difficiles à colorier, optimisant la recherche.
- Améliore la rapidité de résolution en limitant les essais inutiles.

## DSaturSolver 
#### Implémente la méthode DSATUR pour prioriser les cellules à traiter :
- Calcule le degré de saturation de chaque cellule (nombre de couleurs différentes déjà placées dans les cases voisines).
- Sélectionne la cellule avec la saturation maximale.
- Explore les couleurs possibles dans un ordre optimisé.
#### Avantages :
- Plus performant pour les grilles partiellement remplies.
- Exploite les spécificités du Sudoku pour améliorer l’efficacité.


## Graph.cs et SudokuConnections.cs 

#### 1. Graph.cs – Structure du graphe
Ce fichier définit un graphe représentant un Sudoku, avec deux classes principales :

Classe Node (Nœud) : représente une cellule avec un identifiant unique et ses connexions.
Classe Graph : gère l’ensemble des sommets et leurs relations.
Méthodes pour ajouter des nœuds, créer des connexions et vérifier les voisins.
#### 2. SudokuConnections.cs – Construction du graphe Sudoku
Utilise Graph.cs pour modéliser les contraintes du Sudoku :

Processus de création :
Génère 81 nœuds pour les cellules.
Établit les connexions selon les règles du Sudoku (lignes, colonnes, blocs 3×3).

<br>
🔹 En résumé : Graph.cs fournit l’infrastructure de base du graphe, tandis que SudokuConnections.cs exploite cette structure pour représenter les contraintes spécifiques d’un Sudoku.


<br><br>






## Instructions
Le fichier de solution "Sudoku.sln" constitue l'environnement de base du travail et s'ouvre dans Visual Studio Community (attention à bien ouvrir la solution et ne pas rester en "vue de dossier").
En l'état, la solution contient:
- Un répertoire Puzzles contenant 3 fichiers de Sudokus de difficultés différentes
- Un projet Sudoku.Shared: il consitue la librairie de base de l'application et fournit la définition de la classe représentant un Sudoku (SudokuGrid) et l'interface à implémenter par les solvers de sudoku (ISudokuSolver).
- Un projet Sudoku.Backtracking qui fournit plusieurs solvers utilisant la méthode simple du backtracking, et qui illustre également l'utilisation de Python depuis le langage c# via  [Python.Net](https://pythonnet.github.io/) grâce aux packages Nuget correspondants.
- Un projet Sudoku.Benchmark de Console permettant de tester les solvers de Sudoku de façon individuels ou dans le cadre d'un Benchmark comparatif. Ce projet référence les projets de solvers, et c'est celui que vous devez lancer pour tester votre code.
