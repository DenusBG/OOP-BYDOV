# Лабораторна робота №21
**Тема:** OCP: гнучкі алгоритми розрахунку (Factory/Strategy)  
**Варіант:** 1 (Служба таксі)

**Виконав:** студент групи [ІПЗ-3/1]  
**ПІБ:** [Будов Денис]


## 1. Мета роботи
Застосувати принцип відкритості/закритості (OCP) для створення гнучкої системи розрахунків за допомогою патернів **Factory Method** та **Strategy**, забезпечивши можливість легкого додавання нових алгоритмів без зміни існуючого коду.

## 2. Завдання (Варіант 1: Служба таксі)
Реалізувати консольну програму для розрахунку вартості поїздки в таксі з різними тарифами:
* **Вхідні дані:** Відстань (км), Час простою (хв).
* **Стратегії (Тарифи):** Economy, Standard, Premium.
* **Вимога OCP:** Продемонструвати додавання нової стратегії (Night) без зміни коду класу-калькулятора.


## 3. Опис архітектури (UML класів)

Для реалізації використано наступні компоненти:

1.  **ITaxiStrategy** — Інтерфейс, що визначає контракт для розрахунку вартості.
2.  **ConcreteStrategies** — Класи `Economy`, `Standard`, `Premium`, `Night`, що реалізують різні алгоритми ціноутворення.
3.  **TaxiStrategyFactory** — Статична фабрика, що створює об'єкт стратегії на основі рядка (string).
4.  **TaxiService** — Контекст, що використовує стратегію для розрахунку, не знаючи деталей реалізації.

### Діаграма класів (Mermaid)

```mermaid
classDiagram
    class ITaxiStrategy {
        <<interface>>
        +CalculateCost(distance, idleTime) decimal
    }
    
    class EconomyTaxiStrategy {
        +CalculateCost()
    }
    class StandardTaxiStrategy {
        +CalculateCost()
    }
    class PremiumTaxiStrategy {
        +CalculateCost()
    }
    class NightTaxiStrategy {
        +CalculateCost()
    }

    ITaxiStrategy <|.. EconomyTaxiStrategy
    ITaxiStrategy <|.. StandardTaxiStrategy
    ITaxiStrategy <|.. PremiumTaxiStrategy
    ITaxiStrategy <|.. NightTaxiStrategy

    class TaxiService {
        +CalculateRideCost(distance, idleTime, strategy)
    }
    
    class TaxiStrategyFactory {
        +CreateStrategy(type) ITaxiStrategy
    }

    TaxiService ..> ITaxiStrategy : uses
    TaxiStrategyFactory ..> ITaxiStrategy : creates