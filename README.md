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
Ce fichier définit l'architecture du graphe à travers deux classes principales :  

A. Classe Node (Nœud) 
- Représente une cellule du Sudoku sous forme de sommet.  
- Propriétés :  
  - `Id` : identifiant unique du nœud (1 à 81).  
  - `ConnectedTo` : dictionnaire des connexions avec d'autres nœuds.  
- Méthodes principales :  
  - `AddNeighbour` : établit une connexion avec un autre nœud.  
  - `GetConnections` : récupère la liste des nœuds connectés.  

**B. Classe Graph**  
- Gère l’ensemble du graphe et ses connexions.  
- Propriétés :  
  - `TotalV` : nombre total de sommets.  
  - `AllNodes` : dictionnaire contenant tous les nœuds du graphe.  
- Méthodes principales :  
  - `AddNode` : ajoute un sommet au graphe.  
  - `AddEdge` : crée une connexion entre deux sommets.  
  - `IsNeighbour` : vérifie si deux sommets sont connectés.  

#### 2. SudokuConnections.cs – Construction du graphe Sudoku 
Ce fichier s’appuie sur `Graph.cs` pour modéliser un Sudoku sous forme de graphe.  

- Processus de création :  
  1. `GenerateGraph()` : génère 81 nœuds correspondant aux cellules du Sudoku.  
  2. `ConnectEdges()` : établit les liens entre les cellules selon les règles du Sudoku.  
  3. `WhatToConnect()` : définit les connexions en fonction des contraintes :  
     - Lignes : chaque cellule est reliée aux autres cellules de sa ligne.  
     - Colonnes : chaque cellule est connectée aux autres cellules de sa colonne.  
     - Blocs 3×3 : chaque cellule est reliée aux cellules de son bloc.  


🔹 En résumé : `Graph.cs` fournit l’infrastructure de base du graphe, tandis que `SudokuConnections.cs` exploite cette structure pour représenter les contraintes spécifiques d’un Sudoku.









## Résultats obtenus 



![image](https://github.com/user-attachments/assets/a5eab75c-f02b-41e7-924a-d6052cc53b17)






### [Solvers xxx](Sudoku.Xxx/README.md)
