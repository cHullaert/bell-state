using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using System;
using System.Linq;

namespace Quantum.Bell_state
{
    class Driver
    {


        static void runMeasure(QuantumSimulator qsim, Result initial)
        {
            var res = MeasureQBit.Run(qsim, 1000, initial).Result;
            var (numZeros, numOnes, agrees) = res;
            System.Console.WriteLine(
                $"Init:{initial,-4} 0s={numZeros,-4} 1s={numOnes,-4} agree={agrees,-4}");
        }

        static void runFlip(QuantumSimulator qsim, Result initial)
        {
            var res = FlipQBit.Run(qsim, 1000, initial).Result;
            var (numZeros, numOnes, agrees) = res;
            System.Console.WriteLine(
                $"Init:{initial,-4} 0s={numZeros,-4} 1s={numOnes,-4} agree={agrees,-4}");
        }

        static void runSuperposition(QuantumSimulator qsim, Result initial)
        {
            var res = SuperposeQBit.Run(qsim, 1000, initial).Result;
            var (numZeros, numOnes, agrees) = res;
            System.Console.WriteLine(
                $"Init:{initial,-4} 0s={numZeros,-4} 1s={numOnes,-4} agree={agrees,-4}");
        }

        static void runEntanglement(QuantumSimulator qsim, Result initial)
        {
            var res = EntangleQBits.Run(qsim, 1000, initial).Result;
            var (numZeros, numOnes, agrees) = res;
            System.Console.WriteLine(
                $"Init:{initial,-4} 0s={numZeros,-4} 1s={numOnes,-4} agree={agrees,-4}");
        }

        static void run(int action)
        {
            // constructs a quantum simulator
            using (var qsim = new QuantumSimulator())
            {
                // initial values
                Result[] initials = new Result[] { Result.Zero, Result.One };
                foreach (Result initial in initials)
                {
                    // run 1000 times
                    // the operation is run asynchronously
                    // note that operation creates a c# class 

                    switch (action)
                    {
                        case 1:
                            runMeasure(qsim, initial);
                            break;
                        case 2:
                            runFlip(qsim, initial);
                            break;
                        case 3:
                            runSuperposition(qsim, initial);
                            break;
                        case 4:
                            runEntanglement(qsim, initial);
                            break;
                    }
                }
            }
        }

        static void estimate(int action)
        {

            var estimator = new ResourcesEstimator();

            switch (action)
            {
                case 6:
                    var res6 = MeasureQBit.Run(estimator, 1000, Result.Zero).Result;
                    break;
                case 7:
                    var res7 = FlipQBit.Run(estimator, 1000, Result.Zero).Result;
                    break;
                case 8:
                    var res8 = SuperposeQBit.Run(estimator, 1000, Result.Zero).Result;
                    break;
                case 9:
                    var res9 = EntangleQBits.Run(estimator, 1000, Result.Zero).Result;
                    break;
            }

            System.Console.WriteLine(estimator.ToTSV());

            System.Console.WriteLine("");
            System.Console.WriteLine("CNOT: The count of CNOT(also known as the Controlled Pauli X gate) gates executed.");
            System.Console.WriteLine("QubitClifford: The count of any single qubit Clifford and Pauli gates executed.");
            System.Console.WriteLine("Measure: The count of any measurements executed.");
            System.Console.WriteLine("R: The count of any single qubit rotations executed, excluding T, Clifford and Pauli gates.");
            System.Console.WriteLine("T: The count of T gates and their conjugates, including the T gate, T_x = H.T.H, and T_y = Hy.T.Hy, executed.");
            System.Console.WriteLine("Depth: Depth of the quantum circuit executed by the Q# operation. By default, only T gates are counted in the depth, see depth counter for details.");
            System.Console.WriteLine("Width: Maximum number of qubits allocated during the execution of the Q# operation.");
            System.Console.WriteLine("BorrowedWidth: Maximum number of qubits borrowed inside the Q# operation.");
        }

        static void teleport()
        {
            using (var qsim = new QuantumSimulator())
            {
                var rand = new System.Random();

                foreach (var idxRun in Enumerable.Range(0, 8))
                {
                    var sent = rand.Next(2) == 0;
                    var received = TeleportClassicalMessage.Run(qsim, sent).Result;
                    System.Console.WriteLine($"Round {idxRun}:\tSent {sent},\tgot {received}.");
                    System.Console.WriteLine(sent == received ? "Teleportation successful!!\n" : "\n");
                }
            }
        }

        static void playAction(int action)
        {
            if (Enumerable.Range(1, 4).Contains(action))
            {
                run(action);
            }
            else if (Enumerable.Range(6, 4).Contains(action))
            {
                estimate(action);
            }
            if (action == 5)
            {
                teleport();
            }
        }


        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("What do you want to do ?");
            Console.WriteLine("");
            Console.WriteLine("Quantum simulator");
            Console.WriteLine("=================");
            Console.WriteLine("1. Measure qbit");
            Console.WriteLine("2. Flip qbit");
            Console.WriteLine("3. Put qbit in superposition state");
            Console.WriteLine("4. Entangle qbits");
            Console.WriteLine("5. Teleport qbit");
            Console.WriteLine("");
            Console.WriteLine("Resource estimators");
            Console.WriteLine("===================");
            Console.WriteLine("6. Measure qbit");
            Console.WriteLine("7. Flip qbit");
            Console.WriteLine("8. Put qbit in superposition state");
            Console.WriteLine("9. Entangle qbits");
            Console.WriteLine("");
            Console.WriteLine("Please introduce your choice: ");

            var consoleKey = Console.ReadKey();
            if (char.IsDigit(consoleKey.KeyChar)) {
                Console.WriteLine("your choice was: " + consoleKey.KeyChar.ToString());
                playAction(Int32.Parse(consoleKey.KeyChar.ToString()));
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}