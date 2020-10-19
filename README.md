# 1- Counting bits
La manera más eficiente de realizar este ejercicio es mediante el uso de operadores a nivel de bit. Utilizando el desplazamiento lateral derecho se consigue fácilmente iterar sobre los bits del número entero e ir anotando en que posiciones hay un 1.

# 2- Refactoring fraud detection
Una refactorización evidente de este ejercicio es aplicar el principio de responsabilidad única. Para ello empezamos por mover el código de lectura de ficheros a una clase diferente. En este caso he aplicado el patrón repositorio por si en el futuro se quiere extender la aplicación para hacerla compatible con bases de datos. Este cambio nos permite además cambiar la *signature* de la función Check de FraudRadar para recibir un IEnumerable de Orders.

Como medidas defensivas he realizado ciertas comprobaciones a la hora de leer el fichero, por ejemplo el número de elementos del pedido es el esperado o si el email es válido. Aunque en la mayoría de casos ignoro un pedido que no es válido y lo *loggeo* como un aviso, probablemente se debería hacer como un error o incluso lanzar una excepción.

Por último en los tests utilizo la librería *Moq* para simular la configuración y el *logging*. Probablemente es mejor generar el contenido en el código en vez de leer del archivo en cada prueba para minimizar las dependencias en los tests. En este caso como lectura de ficheros es una operación muy sencilla y los ficheros se incluyeron para usarlos lo he dejado como está. 