namespace Quantum.Bell_state
{
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Canon;
    
	// Unit is similar to void
	// this code defines a Q# operation, it's the basic unit of quantum execution in Q# (similar to function)
    operation Set (desired: Result, q1: Qubit) : Unit {
		// M measures the qbit
		if (desired != M(q1)) {
			// X applies a bit flip
			X(q1);
		}    
	}

	// q# use tuples instead of structure or records to pass multiple parameters (Int, Int)
	operation MeasureQBit (count : Int, initial: Result) : (Int, Int, Int) {
		// by default variables are immutable
		// variable types are always inferred by the compiler
		mutable numOnes = 0;

		// initializatino of qbits
		using (qubit = Qubit()) {

			// no equivalent for standard for loop, we have to specify a range
			for (test in 1..count) {
				Set (initial, qubit);

				// mesure the value of the qbits 
				let res = M (qubit);

				// Count the number of ones we saw:
				if (res == One) {
					set numOnes += 1;
				}
			}
			Set(Zero, qubit);
		} // qbits are freed here

		// Return number of times we saw a |0> and number of times we saw a |1>
		return (count-numOnes, numOnes, 0);
	}

	// q# use tuples instead of structure or records to pass multiple parameters (Int, Int)
	operation FlipQBit (count : Int, initial: Result) : (Int, Int, Int) {
		// by default variables are immutable
		// variable types are always inferred by the compiler
		mutable numOnes = 0;

		// initializatino of qbits
		using (qubit = Qubit()) {

			// no equivalent for standard for loop, we have to specify a range
			for (test in 1..count) {
				Set (initial, qubit);

				// flip the QBit
				X(qubit);
				// mesure the value of the qbits 
				let res = M (qubit);

				// Count the number of ones we saw:
				if (res == One) {
					set numOnes += 1;
				}
			}
			Set(Zero, qubit);
		} // qbits are freed here

		// Return number of times we saw a |0> and number of times we saw a |1>
		return (count-numOnes, numOnes, 0);
	}

	// q# use tuples instead of structure or records to pass multiple parameters (Int, Int)
	operation SuperposeQBit (count : Int, initial: Result) : (Int, Int, Int) {
		// by default variables are immutable
		// variable types are always inferred by the compiler
		mutable numOnes = 0;

		// initializatino of qbits
		using (qubit = Qubit()) {

			// no equivalent for standard for loop, we have to specify a range
			for (test in 1..count) {
				Set (initial, qubit);

				// put the QBit in superposed state with the Hadamard gate
				H(qubit);
				// mesure the value of the qbits 
				let res = M (qubit);

				// Count the number of ones we saw:
				if (res == One) {
					set numOnes += 1;
				}
			}
			Set(Zero, qubit);
		} // qbits are freed here

		// Return number of times we saw a |0> and number of times we saw a |1>
		return (count-numOnes, numOnes, 0);
	}

    operation EntangleQBits (count : Int, initial: Result) : (Int, Int, Int) {

        mutable numOnes = 0;
        mutable agree = 0;
        using ((q0, q1) = (Qubit(), Qubit())) {
            for (test in 1..count)
            {
                Set (initial, q0);
                Set (Zero, q1);

                H(q0);
                CNOT(q0,q1);
                let res = M (q0);

                if (M (q1) == res) {
                    set agree += 1;
                }

                // Count the number of ones we saw:
                if (res == One) {
                    set numOnes += 1;
                }
                
            }
            
            Set(Zero, q0);
            Set(Zero, q1);
        }

        // Return number of times we saw a |0> and number of times we saw a |1>
        return (count-numOnes, numOnes, agree);    
	}

	// send the state of one qubit to a target qubit by teleportation
	// the state of msg will be collapsed
	// msg: the qbit we want to send
	// target: the qbit init with |0> (used to get the state of msg)
    operation Teleport (msg : Qubit, target : Qubit) : Unit {
        using (register = Qubit()) {
            
			// Entanglement
            H(register);
            CNOT(register, target);

            // Encode the message into the entangled pair,
            // and measure the qubits to extract the classical data
            // we need to correctly decode the message into the target qubit:
            CNOT(msg, register);
            H(msg);
            let data1 = M(msg);
            let data2 = M(register);

            // apply correction on message
            if (data1 == One) { Z(target); }
            if (data2 == One) { X(target); }

            // Reset our "register"
            Reset(register);
        }
    }
    

	// message represents the qbit values
	// return the z-basis measurement on the teleport qbits (converted in bool)
    operation TeleportClassicalMessage (message : Bool) : Bool {

        using ((msg, target) = (Qubit(), Qubit())) {

            // Encode the message we want to send.
            if (message) {
                Set(One, msg);
            }

            // Use the operation to teleport to target
            Teleport(msg, target);

            // check the value
            let measurement = M(target) == One;

            // Reset all qbits
            Reset(msg);
            Reset(target);

            return measurement;
        }
    }
}

