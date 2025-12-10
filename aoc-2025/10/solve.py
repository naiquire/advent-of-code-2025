import re as Regex
from ortools.sat.python import cp_model

def parseInput(path):
    machines = []

    # use regex to extract data because im losing my sanity
    regexLine = Regex.compile(r'\((.*?)\)|\{(.*?)\}')

    with open(path, 'r', encoding='utf-8') as file:
        for rawLine in file:
            line = rawLine.strip()
            if not line:
                continue

            switches = []
            targetJoltages = None

            for group in regexLine.findall(line):
                isInsideBrackets, isInsideBraces = group

                if isInsideBrackets:
                    # found a switch
                    nums = tuple(int(x) for x in isInsideBrackets.split(','))
                    switches.append(nums)
                    
                elif isInsideBraces:
                    # found the target joltages
                    targetJoltages = tuple(int(x) for x in isInsideBraces.split(','))

            machines.append((switches, targetJoltages))

    return machines

def solve2(switches, targetJoltages):
    switchCount = len(switches)
    joltagesCount = len(targetJoltages)
    target = list(targetJoltages)

    # create new simplex model
    model = cp_model.CpModel()

    # creates objective function
    x = []
    for i in range(switchCount):
        # NewIntVar class takes parameters ~ lower_bound, upper_bound, name
        # number of switch presses will never exceed the total joltage
        x.append(model.NewIntVar(0, sum(targetJoltages), f'x[{i}]'))
    
    # configure constraints
    for i in range(joltagesCount):
        affecting = []
        for j, switch in enumerate(switches):
            if i in switch:
                affecting.append(x[j])
        # adds the switches that can affect the current joltage
        model.Add(sum(affecting) == target[i])

    # minimise the model with set objective function
    model.Minimize(sum(x))

    # solve the model
    solver = cp_model.CpSolver()
    solver.Solve(model)
    
    total = 0
    for value in x:
        total += solver.Value(value)

    return total

def main():

    machines = parseInput("input.txt")
    total = 0

    for idx, (buttons, target) in enumerate(machines, 1):
        subtotal = solve2(buttons, target)
        total += subtotal
        print('\r' + str(total), end="")

    print('\r' + str(total))

main()
