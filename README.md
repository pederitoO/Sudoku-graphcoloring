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
Chaque groupe est invité à créer un [Fork](https://docs.github.com/en/get-started/quickstart/fork-a-repo) de ce dépôt principal muni d'un compte sur Github, et d'indiquer dans le fil de suivi de projet du groupe sur le forum son adresse. 

Vous pourrez ensuite travailler de façon collaborative sur ce fork  en  [attribuant les permissions d'éditions](https://docs.github.com/en/account-and-profile/setting-up-and-managing-your-github-user-account/managing-access-to-your-personal-repositories/inviting-collaborators-to-a-personal-repository) aux autres membres du groupe, en clonant votre fork sur vos machines, par le biais de validations (commits), de push pour remonter les validations sur le server, et de pulls/tirages sur les machines locales des utilisateurs du groupe habilités sur le fork. 

Le plus simple sera d'utiliser les [outils intégrés à Github](https://docs.microsoft.com/fr-fr/visualstudio/version-control/git-with-visual-studio?view=vs-2019) directement disponible depuis l'environnement [Visual Studio](https://visualstudio.microsoft.com/fr/downloads/). Idéalement, préférez Visual Studio Community 2019 ou Visual Studio pour Mac à Visual Studio Code, qui est plus versatile, mais sans doute moins adapté à ce projet. Si vous choisissez toutefois d'utiliser Visual Studio Code, il vous faudra suivre les instructions fournies [pour la prise en charge de c#](https://code.visualstudio.com/docs/languages/csharp) ou [pour celle de .Net](https://code.visualstudio.com/docs/languages/dotnet), et sans doute installer des extensions comme [celle permettant de charger des solutions](https://marketplace.visualstudio.com/items?itemName=fernandoescolar.vscode-solution-explorer). 

Une fois le travail effectué et remonté sur le fork, vous pourrez créer une pull-request depuis le fork vers le dépôt principal pour fusion et évaluation.

Le fichier de solution "Sudoku.sln" constitue l'environnement de base du travail et s'ouvre dans Visual Studio Community (attention à bien ouvrir la solution et ne pas rester en "vue de dossier").
En l'état, la solution contient:
- Un répertoire Puzzles contenant 3 fichiers de Sudokus de difficultés différentes
- Un projet Sudoku.Shared: il consitue la librairie de base de l'application et fournit la définition de la classe représentant un Sudoku (SudokuGrid) et l'interface à implémenter par les solvers de sudoku (ISudokuSolver).
- Un projet Sudoku.Backtracking qui fournit plusieurs solvers utilisant la méthode simple du backtracking, et qui illustre également l'utilisation de Python depuis le langage c# via  [Python.Net](https://pythonnet.github.io/) grâce aux packages Nuget correspondants.
- Un projet Sudoku.Benchmark de Console permettant de tester les solvers de Sudoku de façon individuels ou dans le cadre d'un Benchmark comparatif. Ce projet référence les projets de solvers, et c'est celui que vous devez lancer pour tester votre code.

Il s'agira pour chaque groupe de travail, sur le modèle du projet backtracking en exemple, de commencer par:

- [rajouter à la solution votre propre projet](https://docs.microsoft.com/fr-fr/visualstudio/get-started/tutorial-projects-solutions?view=vs-2019), typiquement une bibliothèque de classes en c# au format .Net standard 2.1,
- munir votre projet des librairies nécessaires, sous forme de [références](https://docs.microsoft.com/fr-fr/visualstudio/ide/managing-references-in-a-project?view=vs-2019) ou de packages [Nuget](https://docs.microsoft.com/fr-fr/nuget/consume-packages/install-use-packages-visual-studio). Votre projet devrait posséder une référence vers le projet partagé contenant la définition de la classe d'une grille de Sudoku et de l'interface d'un solver de Sudoku.
- Créer votre classe de Solver (qui n'a pas besoin d'être entièrement fonctionnelle lors de vos premiers archivages) implémentant l'interface partagée.
- faire référencer votre projet par celui de l'application de Console Benchmark pour le rendre votre solver disponible dans le menu.

Pour ce qui est du Workflow de collaboration:

- Effectuez régulièrement une validation/commit pour baliser votre travail localement,
- puis un push pour le remonter vos validations sur votre fork et le partager avec les camarades de votre groupe, qui pourront le récupérer sur leurs propres environnements avec un tirage/pull.
- Lorsque votre environnement est stable (la solution compile sans erreurs et les autres projets ne sont pas affectés par le vôtre), effectuez une [pull-request](https://docs.github.com/en/github/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/about-pull-requests)/requête de tirage vers le dépôt principal pour partager votre travail avec moi et les autres groupes, ainsi que dans l'autre sens à n'importe quel moment que pour récupérer les dernières mises à jours remontées par vos camarades sur le dépôt principal (dont je m'assurerai de la bonne stabilité). Pour effectuer et valider des pull requests dans un sens ou dans l'autre, vous pouvez utiliser l'[interface de github](https://docs.github.com/en/github/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/creating-a-pull-request-from-a-fork), ou encore [celle de Visual Studio](https://visualstudio.developpez.com/actu/261500/Pull-Requests-pour-Visual-Studio-une-fonctionnalite-collaborative-devoilee-avec-Visual-Studio-2019-pour-gerer-les-demandes-de-tirage-dans-l-EDI/).
- Vous serez invité à vous évaluer entre autre sur votre capacité à participer au projet global en partageant régulièrement votre travail et en récupérant celui partagé par les autres groupes. Tout cela sera visible dans la page Insights/Network de vôtre dépôt ou du dépôt principal.

Concernant enfin vos sujets respectifs et vos éventuelles questions spécifiques, je vous invite à utiliser le fil de discussion de votre groupe dans le forum du projet.





