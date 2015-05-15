Synchronic World Service
======================

M1 Project - Supinfo Nice 2015<br/>
***.NET - Teacher : Sir Philippe Vialatte***

* * *
##AIM of this project ?##
>You work for a small company specialized in event organization: “Synchronic World”. The commercial team wants to create a new product. They are asking you to create a PoC (Proof of Concept or prototype).
You will work on a new event management system. Inspired by a world famous social network, you will create an alternative supervision system, based on a web service.
The development team is actually working on a new Web Service system. At the end all the data layer should be consume by many devices.
You are in charge of the development of a new Web Service and a small console application to test this system.

* * *
##What have been implemented##
* WCF service : **SynchronicWorldService project**
* Console consumer : **SynchronicWorldConsole project**

* * *
##Technologic key points of this project ?##
* **Resources** file (resx, inside Utils project)
* **Log4Net** (Configured inside Utils project according to the config file stored inside the SynchronicWorldService project)
* **WCF through TCP**
* **WCF multiple endpoints**
* **Entity Framework**
* **Unit Of Work**, for db context (using feature)
* **Effort testing**, for integration testing
* **NUnit**, Unit testing framework
* **Dependencies injection**, thanks to Microsoft.Practices.Unity dll

* * *




