"""Sort a list using merge sort"""

import argparse


def sort(list_in):
    """Sort a sublist using merge sort"""

    # List of one item, no need to sort
    if len(list_in) == 1:
        return list_in

    # Split list in half
    list1 = sort(list_in[:len(list_in) // 2])
    list2 = sort(list_in[len(list_in) // 2:])

    # Initialize result list and counters
    list_out = []
    count1 = 0
    count2 = 0

    # Loop until one of the sublists is exhausted (merge sort)
    while count1 < len(list1) and count2 < len(list2):
        # Set match
        candidate1 = list1[count1]
        candidate2 = list2[count2]
        print('(1) ' + str(candidate1))
        print('versus')
        print('(2) ' + str(candidate2))

        # Wait for valid user input
        answer = input()
        if answer == '1':
            # First candidate preferred
            list_out.append(candidate1)
            count1 += 1
        elif answer == '2':
            # Second candidate preferred
            list_out.append(candidate2)
            count2 += 1
        else:
            # Invalid input, loop again
            print('Invalid input, try again')

    # For the non-exhausted list, add remaining candidates
    for i in range(count1, len(list1)):
        list_out.append(list1[i])
    for i in range(count2, len(list2)):
        list_out.append(list2[i])

    return list_out

# Set parser arguments
parser = argparse.ArgumentParser()
parser.add_argument('-in',
    default='input.txt',
    help='Input file, default is input.txt',
    dest='filename_in')
parser.add_argument('-out',
    default='output.txt',
    help='Output file, default is output.txt',
    dest='filename_out')
args = parser.parse_args()

# Welcome message
print('Welcome to the favorites sorter')
print('You are given some match-ups')
print('Select your preference by typing 1 or 2')
print('At the end, the program returns the order of preference')
print('')
print('Data taken from: ' + args.filename_in)
print('Result will be written to: ' + args.filename_out)

# Read input file
candidates = []
with open(args.filename_in, 'r', encoding='utf-8') as file_in:
    for line in file_in.readlines():
        # Remove whitespace
        stripped_line = line.strip()
        if stripped_line:
            # If blank line, ignore
            candidates.append(stripped_line)
file_in.close()

# Sort list
if len(candidates) < 2:
    # Zero or one element, no need to sort
    result = candidates
else:
    result = sort(candidates)
print('The order of preference is:')
print(result)

# Write to output file
with open(args.filename_out, 'w', encoding='utf-8') as file_out:
    for line in result:
        file_out.write(line + '\n')
    file_out.close()
