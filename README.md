Maciej Skorupski										05.07.2024
Inżynieria Systemów										

**Generowanie losowego układu pomieszczeń
w przestrzeni trójwymiarowej**
























**Spis Treści:**

- Wstęp
- Wprowadzenie do omawianego problemu
- Tworzenie trójwymiarowej siatki
- Generowanie pomieszczeń
- Łączenie wierzchołków grafu
- Redukcja połączeń między wierzchołkami
- Znalezienie drogi pomiędzy dwoma wierzchołkami
- Wnioski

























**Wstęp:**

Celem tego sprawozdania jest przedstawienie metody łączenia losowo rozmieszczonych pomieszczeń w przestrzeni trójwymiarowej. Proces ten umożliwia tworzenie różnorodnych struktur bez konieczności ręcznego modelowania dróg między nimi. System ten jest szczególnie użyteczny w produkcji gier komputerowych, gdzie każda nowa sesja może oferować graczowi unikalne doznania. Do automatycznego generowania struktur i wizualizacji rezultatów wykorzystałem silnik Unity oraz język programowania C#.


**Wprowadzenie do omawianego problemu:**

`	`Generowanie losowego układu pomieszczeń w przestrzeni i symulacja 3D może stanowić wyzwanie z następujących powodów:

\- Po pierwsze, wygenerowane struktury muszą stanowić razem spójną całość. Wygenerowane pomieszczenia i drogi pomiędzy nimi nie mogą się wzajemnie przecinać, gracz w swobodny sposób powinien być w stanie przemieszczać się przez stworzoną przez nas mapę. 

\- Po drugie,  zaproponowana mapa za każdym razem musi stanowić ciekawe i unikalne doświadczenie dla gracza.

W celu zapewnienia wyżej obranych wymagań, skorzystamy z koncepcji grafu, gdzie pomieszczenia reprezentowane będą jako wierzchołki (Node), natomiast połączenia między nimi jako krawędzie (Edge). Struktura ta umożliwi nam w łatwy sposób nawigację i zarządzanie zdefiniowaną ówcześnie przestrzenią.

Definicja krawędzi:

Zaproponowana przeze mnie klasa pozwoli nam opisać połączenie między dwoma wierzchołkami, natomiast funkcja *DrawFinalLines* pozwoli na wizualizacje ów połączenia.





Definicja wierzchołka:

Zamodelowana przeze mnie klasa zawierająca między innymi informacje na temat położenia wierzchołka zarówno w siatce jak i przestrzeni, informacje czy dany wierzchołek może posłużyć za drogę łączącą dwa pokoje, lub jaki obiekt znajduje się w miejscu naszego wierzchołka, jaki i reprezentowany przez niego typ. 

Opisanie przestrzeni w ten sposób umożliwi mi w łatwiejszy sposób opisanie zależności między stworzonymi pokojami i łączącymi je korytarzami.








**Tworzenie trójwymiarowej siatki:**

W celu uporządkowania przestrzeni operacyjnej opiszemy ją przy użyciu trójwymiarowej tablicy zawierającej dane o wszystkich możliwych wierzchołka w niej zawartych.

` `W obranym przeze mnie podejściu funkcja *CreateGrid* inicjalizuje wierzchołki w siatce o dowolnie ustalonych parametrach, jednocześnie przypisując każdemu z nich odpowiednią pozycje w  opisywanej przez nas przestrzeni jak i na samej siatce. Ostatecznie możemy pomyśleć o tej przestrzeni jako o sześcianie stworzonym z mniejszych sześcianów, gdzie każdy mniejszy sześcian reprezentuje instancje odpowiedniego wierzchołka. 

Na siatce zdefiniować możemy również inne funkcje które przydatne będą w kontekście późniejszych rozważań, jedną z takich funkcji jest:

- *NodeFromWorldPosition* – funkcja ta umożliwia sparsowanie danych na temat pozycji wierzchołka w świecie na informacje o pozycji wierzchołka w trójwymiarowej siatce.
- *GetNeighboringCells* – funkcja zwracająca nam listę wierzchołków bezpośrednio stycznych do wybranego wierzchołka.

Wizualizacja Przestrzeni o wymiarach 300x50x300, gdzie biały sześcian
reprezentuje jeden wierzchołek


**Generowanie pomieszczeń:**
**
`	`Dla zachowania większej kontroli nad wyglądem pokoi, generowane będą z ówcześnie zdefiniowanej listy, gotowych pomieszczeń. Pozwoli nam to na posiadanie większego wpływu nad wrażeniami i płynnością rozgrywki. System ten jednak musi być z łatwością skalowalny, aby w przyszłości nie utrudniał skalowania projektu, aby zdefiniować ile wierzchołków w przestrzeni wprowadzę kolejną klasę obiektu (Room).

Klasa ta zawiera informacje o wielkości pokoju jak i o dostępnych wyjściach/wejściach. Co w przyszłości pozwoli na uniknięcie błędu związanego z tym ,iż kilka dróg może dążyć do tego samego pokoju w jeden sposób co prowadzić by mogło do nakładania się wzajemnie wielu przejść. 


Aby zainicjować pokoje w świecie, należy wcześniej zdefiniować ich maksymalną ilość jak i ramy powierzchni na której możliwe będzie ich ustawienie, jak i minimalną odległość między dwoma pokojami.

Następnie, wybieramy jeden z wcześniej przygotowanych schematów pokoi i losowe miejsce w przestrzeni, które odnosi się do odpowiedniego wierzchołka w trójwymiarowej siatce. Przed ostatecznym stworzeniem obiektu na mapie, sprawdzamy czy nie przecina się on z innym pokojem przy użyciu funkcji *CheckBoundires* oraz czy nie nie wychodzi on poza granice obranego przez nasz obszaru. 

Funkcja *CheckBoundires* stwarza sferę która, zwraca wartość *false* jeżeli w ówcześnie zdefiniowanej odległości *roomSpacing* nie znajduje się żaden obiekt, pozwoli nam to zachować odpowiednią przestrzeń między pokojami co wykluczy ewentualne błędny przy generacji związane z nachodzeniem na siebie dwóch pokoi.


Jeżeli, we wcześniejszych krokach nie natknęliśmy się naprzeciw wskazania przed stworzeniem pokoju, w obranym wcześniej miejscu, możemy stworzyć jego instancje na mapie. Następnym krokiem który musi obrać jest zmapowanie wyjść z pokoju do odpowiednich wierzchołków w siatce, korzystamy tutaj z ówcześnie przedstawionej funkcji *NodeFromWorldPosition* , która pozwoli nam na odwołanie się do odpowiedniego wierzchołka przy użyciu pozycji w przestrzeni. 

Kolejnym krokiem jest zredefiniowanie właściwości wierzchołków będących w obrębie postawionego pokoju, oznacza to iż musimy przejść przez wszystkie wierzchołki oddalone od pierwotnie obranego punktu o długość, szerokość i wysokość stawianego pokoju, w odpowiednich płaszczyznach. Procedura ta zapewni, poprawność generacji korytarzy łączących pokoje.

Wizualizacja wierzchołów przez których właściwości zostały zmienione w poprzednim kroku.

Przykładowe rozłożenie pokoi w przestrzeni.


**Łączenie wierzchołków grafu:**
**
`	`W celu zapewnienia iż będzie istniało poprawne przejście pomiędzy dwoma pokojami, postanowiłem obliczyć środek współrzędne środka sfery przechodzącej przez każde cztery różne pokoje, po czym sprawdziłem czy w środku utworzonej sfery znajduje się jakikolwiek inny pokój. W przypadku gdy środek sfery będzie pusty, stworzyłem wzajemne krawędzie między wierzchołkami w tym przypadku pokojami.






Funkcja *AddNodesToLinkedNodesList*, służy do powiązania wierzchołków między sobą nawzajem.




Funkcja *CalculateCircumsphere* służy do opisania wcześniej przytoczonej sfery. 

Przedstawiona powyżej funkcja przy użyciu macierzy 4x4 zawierającej pozycje punktów w przestrzeni służy do rozwiązania równania sfery w postaci:

x-a2+y-b2+z-c2=R2

Gdzie bezpośrednio wyznaczniki macierzy xMatrix, yMatrix, zMatrix, aMatirx, cMatrix służą do obliczenia odpowiednio współrzędnej a, b, c oraz promienia R. Do obliczenia ów wyznaczników zastosowałem metodę *determinant* dostępną w silniku Unity. Ostatecznie wystarczyło podstawić otrzymane wyniki do następujących wzorów:

a=Dx2Da

b=Dy2Da

C=Dz2Da

R=a2+b2+C2-DcDa  lub R = Dx2+Dy2+Dz2-4DaDc2Da





Gdzie odpowiednio:

Dx = x12+y12+z12y1z11x22+y22+z22y2z21x32+y32+z32y3z31x42+y42+z42y4z41

Dy = x12+y12+z12x1z11x22+y22+z22x2z21x32+y32+z32x3z31x42+y42+z42x4z41

Dz = x12+y12+z12x1y11x22+y22+z22x2y21x32+y32+z32x3y31x42+y42+z42x4y41

Dc = x12+y12+z12x1y1z1x22+y22+z22x2y2z2x32+y32+z32x3y3z3x42+y42+z42x4y4z4

Da = x1y1z11x2y2z21x3y3z31x4y4z41

Jeżeli jedynymi obiektami wewnątrz naszej sfery są jedynie cztery pokoje i wyznacznik macierzy Da jest różny od zera, uznajemy wtedy je za poprawną czwórkę  i tworzymy między nimi krawędzie, co w późniejszych rozważaniach pozwoli wygenerować między nimi ścieżki.













**Redukcja połączeń między wierzchołkami:**

W celu redukcji połączeń między wierzchołkami zamierzam użyć zmodyfikowanej wersji algorytmu Prim’a dla otrzymania minimalnego rozpięcia drzewa. 

Algorytm ten zaczyna od dodania pierwszego wierzchołka z listy *nodeList* zawierającej informacje na temat wierzchołków wybranych za środek pokoju w trakcie ich tworzenia, następnie będzie tworzyć kolejne krawędzie między wierzchołkami dodając je do listy *finalEdgeList* do czasu aż jej długość nie będzie równała się ilości pokoi minus jeden. Odpowiednie krawędzie wybierane i tworzone są w funkcji *CurrentPossibleEdges.*

Funkcja ta przeszukuje wszystkie możliwe krawędzie wychodzące z węzłów zawartych w liście *activatedNodes*, tworząc krawędź między pokojami o najmniejszej wadze (odległościami), aby zapewnić optymalne ułożenie korytarzy wyjścia z dwóch pokoi dobierane są tak aby były one najbliżej siebie.


Wizualizacja krawędzi między pokojami/węzłami





**Znalezienie drogi między wierzchołkami:**

W celu odnalezienia drogi między dwoma wierzchołkami posłużę się algorytmem a\*, służącym do odnajdywania drogi między dwoma punktami w przestrzeni. W przypadku mojej implementacji starałem się zachować ciągły wygląd korytarzy, przez co, wybierać musiałem często nie najoptymalniejszą drogę, gdyż zazwyczaj ta prowadziła przez linię ukośną.
**

Funkcja ta rozpoczyna się inicjalizacją dwóch zbiorów *closeSet* i *openSet,* które przechowywać będą informacje o odwiedzonych jak i potencjalnych wierzchołkach. Algorytm ten  będzie działał aż do czasu odnalezienia węzła końcowego. Jeśli aktualnie sprawdzany węzeł nie jest naszym punktem docelowym, algorytm ocenia jego sąsiadów dodając go do zbioru otwartego jeżeli nie jest zawarty w zbiorze zamkniętym oraz wartość *isWalkable* jest prawdziwa co oznacza iż rozpatrywany wierzchołek nie może zawierać innego korytarza lub pokoju, jeżeli te dwa warunki zostaną spełnione zostaje on dodany do zbioru otwartego oraz nasz aktualny wierzchołek jest ustawiany za jego rodzica (parentNode).

Jeżeli aktualny wierzchołek jest naszym wierzchołkiem docelowym zaczynamy odtwarzać drogę, korzystając z funkcji *TracePath*

Funkcja ta tworzy instancję wcześniej stworzonego obiektu korytarza umieszczając go w miejscu odpowiedniego wierzchołka znajdującego się na drodze między wierzchołkiem początkowym a wierzchołkiem końcowym, przy okazji ustawiając schody umożliwiające przemieszczanie w pionie w razie gdy dwa sąsiadujące wierzchołki znajdują się na innych wartościach w płaszczyźnie y. 

Ponieważ bazowy model korytarza jest sześcianem obudowanym ścianami z każdej ze stron, aby umożliwić bezproblemowe przejście między dwoma pokojami musimy usunąć ściany, nam to uniemożliwiające.

































Bazowy model korytarza.

W tym celu zdefiniowałem funkcje *DeleteWalls* 

Funkcja ta ma na celu wykrycie ścian stojących pomiędzy dwoma sąsiadującymi wierzchołkami. W celu tym wykorzystałem dwa promienie wychodzące z środków odpowiednich wierzchołkach, zwróconych w swoją stronę. Gdy promień natknie się na ścianę zostaje ona usunięta co pozwoli na przejście pomiędzy pokojami.

Przykładowy korytarz obrazujący przejście promienia pomiędzy dwoma korytarzami.

Ten sam korytarz po usunięciu ścian.

**Wnioski:**
**
`	`Implementacja przedstawionego systemu generowania losowego układu pomieszczeń może stanowić szkielet podczas tworzenia zróżnicowanych i interesujących map, które mogą być wykorzystane w np. w grach komputerowych do tworzenia unikalnych doświadczeń dla gracza przy każdej nowej sesji.

Przykładowa mapa na przestrzeni 300x50x300 i rozstawieniu pokoju 50


przykład mapy na przestrzeni 200x400x200 i rozstawieniu pokoju 50



**Bibliografia:**

- <https://vazgriz.com/119/procedurally-generated-dungeons/> 
- <https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW> autorstwa Sebastaina Lague
- <https://mathworld.wolfram.com/Circumsphere.html>

