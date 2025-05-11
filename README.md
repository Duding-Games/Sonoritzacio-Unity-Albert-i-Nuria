# Memòria Projecte de Sonorització d’una Escena en Unity - Albert Cifuentes i Núria Casé

### 1. Descripció General de l'Escena
 L'escena creada representa una petita granja que inclou diversos elements naturals, com un riu, diversos animals i diversos tipus de terres. La finalitat d’aquest projecte és proporcionar una experiència immersiva mitjançant l'ús de l'àudio dinàmic que reacciona a la posició del jugador i als esdeveniments dins de l'escena.

### 2. Implementació de l'Àudio de l'Aigua
 El primer repte va ser implementar l'àudio del riu, el qual envolta i travessa la granja. A diferència d'un llac circular on un sol "Audio Source" podria ser suficient, l'escena requeria una solució més dinàmica. Per aconseguir-ho, es va desenvolupar un script personalitzat que detecta tots els tiles amb el tag “Water” i calcula la posició del jugador en relació amb cada tile. En funció de la distància entre el jugador i l'aigua, el script ajusta el volum d’un "Audio Source" 2D (que no depèn de la posició) per crear un efecte realista d’apropament i allunyament del so del riu.

### 3. Música de Fons
 A continuació, es va afegir música de fons amb un "Audio Source" 2D. Es va ajustar el volum per assegurar-se que la música fos agradable i no molestés l'usuari, mantenint un equilibri entre la immersió i la comoditat auditiva.Tant aquest àudio com el del riu utilitzen el load type: Streaming, ideal per fitxers llargs i continus.

 ### 4. Àudio dels Animals
 Per als animals, es va crear una sèrie d’àudios aleatoritzats amb diversos sorolls d'animals. Aquests sons es van assignar a "Audio Sources" col·locats estratègicament a l'escena, personalitzant diversos paràmetres com el volum, la distància mínima i màxima, el "falloff", entre d’altres, per aconseguir una reproducció més natural dels sons en funció de la ubicació del jugador.

### 5. Sons de Passos
 Els sons dels passos del jugador també es van adaptar a les diferents superfícies del mapa. Mitjançant la creació de tags específics per a cadascuna de les superfícies (terra, fusta, herba, etc.), es va desenvolupar un script que alterna els sons de les petjades segons el tipus de superfície en què es troba el jugador. Aquest efecte aporta un toc de realisme a la interacció del jugador amb l'entorn. A més, quan el jugador camina sobre l'aigua, es canvia el snapshot de l’Audio Mixer per aplicar reverb, augmentant la sensació d’humitat i immersió.

### 6. Sons Ambientals
 Per millorar l'ambient, es va incorporar un loop ambiental amb un "Audio Source" 2D que genera una sensació d'immersió contínua. Aquest so s'ha ajustat perquè el volum sigui agradable i no interferís amb altres elements sonors de l'escena.

### 7. Reverb Zones per a l'Interior de la Granja
 Per a l'interior de l'edifici de la granja, es van afegir zones de reverb que ajusten el so de manera que es simula un canvi en l'acústica quan el jugador es troba dins. Aquest efecte es va aconseguir mitjançant "Reverb Zones" dins de Unity, proporcionant una transició natural entre l'espai exterior i interior.

### 8. Optimització i Postproducció dels Sons
Tots els sons han estat modificats i millorats prèviament amb Audacity, aplicant ajustos de qualitat i generant loops quan ha estat necessari. Pel que fa a la configuració dins de Unity:
Els sons llargs (música i riu) utilitzen el load type: Streaming.
La resta de sons (passos, animals, ambient, etc.) estan configurats amb Decompress on Load, per assegurar una baixa latència i una millor resposta immediata.

### 9. Gestió Avançada de l’Àudio amb Audio Mixer
S’ha configurat un Audio Mixer per dividir els sons en tres grups principals:
Música
Passos (Footsteps)
Entorn (Environment)
Aquest mixer disposa de dos snapshots:
Default: per a la major part de l’escena.
Water: actiu quan el jugador es mou per sobre de superfícies d’aigua, aplicant una capa de reverberació específica per simular l’efecte acústic d’humitat.

### 10.  Postproducció i Edició de Sons
 Tots els sons utilitzats han estat prèviament modificats per millorar-ne la qualitat i adaptació a l'escena. Això inclou l’augment de la qualitat del so, així com la creació de loops a partir de clips originals mitjançant l'eina d'edició d'àudio Audacity. Aquests ajustos han garantit una experiència auditiva més rica i fluida.





