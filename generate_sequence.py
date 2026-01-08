# Generate sequence for Quick Import: 200 points, 15 hours, range 10-2000 PSI
min_psi = 10
max_psi = 2000
points = 200
hold_minutes = 4.5  # 15 hours / 200 points = 4.5 minutes per point

step = (max_psi - min_psi) / (points - 1)
sequence = []

for i in range(points):
    psi = min_psi + (i * step)
    sequence.append(f"{psi:.1f}:{hold_minutes:.1f}")

result = ", ".join(sequence)

# Save to file
with open("quick_import_sequence.txt", "w", encoding="utf-8") as f:
    f.write(result)

print(f"Generated sequence: {points} points")
print(f"Range: {min_psi} - {max_psi} PSI")
print(f"Duration: {points * hold_minutes / 60:.1f} hours")
print(f"\nFirst 5 points:")
for i, point in enumerate(sequence[:5]):
    print(f"  {i+1}. {point}")
print(f"\nLast 5 points:")
for i, point in enumerate(sequence[-5:], start=points-4):
    print(f"  {i}. {point}")
print(f"\nFull sequence saved to: quick_import_sequence.txt")

