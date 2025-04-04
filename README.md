# Parcial_2_Servicios
Segundo parcial de la asignatura Aplicaciones y Servicios Web \
Instituto Tecnológico Metropolitano carrera Tecnología de Desarrollo de Software\
Abril 03 de 2025

## Contenido
- [Descripción del problema](#Descripción-del-problema)
- [Puntos a realizar](#Puntos-a-realizar)
- [Diagrama de la base de datos](#Diagrama-de-la-base-de-datos)
- [Relaciones](#Relaciones)
- [Integrantes](#Integrantes)

## Descripción del problema
La agencia de vías de Antioquia está interesada en una aplicación para pesar los camiones que
circulan por las vías del departamento, para eso ha instalado unas estaciones de pesaje.
Se quiere almacenar los datos del camión, los datos del pesaje y las fotos del proceso para 
que queden como constancia.

## Puntos a realizar
- Debe crear un servicio para grabar el proceso del pesaje y consultar los datos de estos por 
camión, y otro para grabar y consultar las imágenes registradas.
- Se debe permitir ingresar las imágenes, actualizarlas y eliminarlas. 
- Al grabar el proceso del pesaje debe validar si la placa del camión ya existe en la base de datos, 
si no existe se debe grabar. Solo debe tener un servicio para grabar el pesaje, en este 
servicio deben ir los datos del camión.
- Debe hacer una consulta por placa de camión para obtener los procesos de pesaje que ha tenido
- En esta consulta se debe poder ver: 
   1. la placa del camión
   2. el número de ejes
   3. la marca
   4. la fecha del pesaje
   5. el peso obtenido
   6.  el nombre de las imágenes que respaldan el proceso.

## Diagrama de la base de datos
![Diagrama bd](https://github.com/user-attachments/assets/eee47b3f-c161-4f6e-8c2e-0739711b45e9)

## Integrantes
- Manuela Estrada Villada
- Juan Camilo Duarte Vasco

