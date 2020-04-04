# libraries
import matplotlib.pyplot as plt
import numpy as np

# Data
cases = []
activeCases = []
normal = []
incubating = []
infected = []
recovered = []
dead = []
f = open("coronavirusExpansion.txt", "r")
f.readline()
casesData = f.readline().split(", ")
for c in casesData:
    if c.isdigit():
        cases.append(int(c))
activeCasesData = f.readline().split(", ")
for c in activeCasesData:
    if c.isdigit():
        activeCases.append(int(c))
normalData = f.readline().split(", ")
for c in normalData:
    if c.isdigit():
        normal.append(int(c))
incubatingData = f.readline().split(", ")
for c in incubatingData:
    if c.isdigit():
        incubating.append(int(c))
infectedData = f.readline().split(", ")
for c in infectedData:
    if c.isdigit():
        infected.append(int(c))
recoveredData = f.readline().split(", ")
for c in recoveredData:
    if c.isdigit():
        recovered.append(int(c))
deadData = f.readline().split(", ")
for c in deadData:
    if c.isdigit():
        dead.append(int(c))

# Plot
plt.plot(cases, 'c')
plt.plot(activeCases, 'r')
plt.plot(normal, 'g')
# plt.plot(incubating, 'y')
plt.plot(infected, 'r')
plt.plot(recovered, 'b')
plt.plot(dead, 'k')
plt.ylabel('Agents')
plt.xlabel('Days')
plt.show()

plt.plot()
