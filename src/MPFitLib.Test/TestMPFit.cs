/* 
 * MINPACK-1 Least Squares Fitting Library
 *
 * Original public domain version by B. Garbow, K. Hillstrom, J. More'
 *   (Argonne National Laboratory, MINPACK project, March 1980)
 * 
 * Translation to C Language by S. Moshier (moshier.net)
 * Translation to C# Language by D. Cuccia (http://davidcuccia.wordpress.com)
 * 
 * Enhancements and packaging by C. Markwardt
 *   (comparable to IDL fitting routine MPFIT
 *    see http://cow.physics.wisc.edu/~craigm/idl/idl.html)
 */

/* Test routines for MPFit library
   $Id: TestMPFit.cs,v 1.1 2010/05/04 dcuccia Exp $
*/

using System;
using System.Diagnostics;

namespace MPFitLib.Test
{
    public class TestMPFit
    {
        /* Main function which drives the whole thing */
        public static void Main()
        {
            int i;
            int niter = 1;

            for (i = 0; i < niter; i++)
            {
                TestLinFit();
                TestQuadFit();
                TestQuadFix();
                TestGaussFit();
                TestGaussFix();
            }
        }

        /* Test harness routine, which contains test data, invokes mpfit() */
        private static int TestLinFit()
        {
            double[] x = {-1.7237128E+00,1.8712276E+00,-9.6608055E-01,
		        -2.8394297E-01,1.3416969E+00,1.3757038E+00,
		        -1.3703436E+00,4.2581975E-02,-1.4970151E-01,
		        8.2065094E-01};
            double[] y = {1.9000429E-01,6.5807428E+00,1.4582725E+00,
		        2.7270851E+00,5.5969253E+00,5.6249280E+00,
		        0.787615,3.2599759E+00,2.9771762E+00,
		        4.5936475E+00};

            double[] ey = new double[10];
            double[] p = { 1.0, 1.0 };           /* Initial conditions */
            double[] pactual = { 3.20, 1.78 };   /* Actual values used to make data */
            //double[] perror = { 0.0, 0.0 };                   /* Returned parameter errors */
            int i;
            int status;

            mp_result result = new mp_result(2);
            //result.xerror = perror;

            for (i = 0; i < 10; i++)
            {
                ey[i] = 0.07; /* Data errors */
            }

            CustomUserVariable v = new CustomUserVariable();
            v.X = x;
            v.Y = y;
            v.Ey = ey;

            /* Call fitting function for 10 data points and 2 parameters */
            status = MPFit.Solve(ForwardModels.LinFunc, 10, 2, p, null, null, v, ref result);

            Debug.Print("*** TestLinFit status = {0}\n", status);
            PrintResult(p, pactual, result);

            return 0;
        }

        /* Test harness routine, which contains test quadratic data, invokes
           Solve() */
        private static int TestQuadFit()
        {
            double[] x = {-1.7237128E+00,1.8712276E+00,-9.6608055E-01,
		        -2.8394297E-01,1.3416969E+00,1.3757038E+00,
		        -1.3703436E+00,4.2581975E-02,-1.4970151E-01,
		        8.2065094E-01};
            double[] y = {2.3095947E+01,2.6449392E+01,1.0204468E+01,
		        5.40507,1.5787588E+01,1.6520903E+01,
		        1.5971818E+01,4.7668524E+00,4.9337711E+00,
		        8.7348375E+00};
            double[] ey = new double[10];
            double[] p = { 1.0, 1.0, 1.0 };        /* Initial conditions */
            double[] pactual = { 4.7, 0.0, 6.2 };  /* Actual values used to make data */
            //double[] perror = new double[3];		       /* Returned parameter errors */
            int i;
            int status;

            mp_result result = new mp_result(3);
            //result.xerror = perror;

            for (i = 0; i < 10; i++)
            {
                ey[i] = 0.2;       /* Data errors */
            }

            CustomUserVariable v = new CustomUserVariable() { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 3 parameters */
            status = MPFit.Solve(ForwardModels.QuadFunc, 10, 3, p, null, null, v, ref result);

            Debug.Print("*** TestQuadFit status = {0}\n", status);
            PrintResult(p, pactual, result);

            return 0;
        }

        /* Test harness routine, which contains test quadratic data;
           Example of how to fix a parameter
        */
        private static int TestQuadFix()
        {
            double[] x = {-1.7237128E+00,1.8712276E+00,-9.6608055E-01,
		-2.8394297E-01,1.3416969E+00,1.3757038E+00,
		-1.3703436E+00,4.2581975E-02,-1.4970151E-01,
		8.2065094E-01};
            double[] y = {2.3095947E+01,2.6449392E+01,1.0204468E+01,
		5.40507,1.5787588E+01,1.6520903E+01,
		1.5971818E+01,4.7668524E+00,4.9337711E+00,
		8.7348375E+00};

            double[] ey = new double[10];
            double[] p = { 1.0, 0.0, 1.0 };        /* Initial conditions */
            double[] pactual = { 4.7, 0.0, 6.2 };  /* Actual values used to make data */
            //double[] perror = new double[3];		       /* Returned parameter errors */
            int i;
            int status;

            mp_result result = new mp_result(3);
            //result.xerror = perror;

            mp_par[] pars = new mp_par[3] /* Parameter constraints */
                                {
                                    new mp_par() , 
                                    new mp_par(){isFixed = 1},  /* Fix parameter 1 */
                                    new mp_par()
                                };             

            for (i = 0; i < 10; i++)
            {
                ey[i] = 0.2;
            }

            CustomUserVariable v = new CustomUserVariable() {X = x, Y = y, Ey = ey};

            /* Call fitting function for 10 data points and 3 parameters (1
               parameter fixed) */
            status = MPFit.Solve(ForwardModels.QuadFunc, 10, 3, p, pars, null, v, ref result);

            Debug.Print("*** TestQuadFix status = {0}\n", status);

            PrintResult(p, pactual, result);

            return 0;
        }


        /* Test harness routine, which contains test gaussian-peak data */
        private static int TestGaussFit()
        {
            double[] x = {-1.7237128E+00,1.8712276E+00,-9.6608055E-01,
		-2.8394297E-01,1.3416969E+00,1.3757038E+00,
		-1.3703436E+00,4.2581975E-02,-1.4970151E-01,
		8.2065094E-01};
            double[] y = {-4.4494256E-02,8.7324673E-01,7.4443483E-01,
		4.7631559E+00,1.7187297E-01,1.1639182E-01,
		1.5646480E+00,5.2322268E+00,4.2543168E+00,
		6.2792623E-01};
            double[] ey = new double[10];
            double[] p = { 0.0, 1.0, 1.0, 1.0 };       /* Initial conditions */
            double[] pactual = { 0.0, 4.70, 0.0, 0.5 };/* Actual values used to make data*/
            //double[] perror = new double[4];			   /* Returned parameter errors */
            mp_par[] pars = new mp_par[4] /* Parameter constraints */
                                {
                                    new mp_par(),
                                    new mp_par(), 
                                    new mp_par(),
                                    new mp_par()
                                }; 
            int i;
            int status;

            mp_result result = new mp_result(4);
            //result.xerror = perror;

            /* No constraints */

            for (i = 0; i < 10; i++) ey[i] = 0.5;

            CustomUserVariable v = new CustomUserVariable() { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 4 parameters (no
               parameters fixed) */
            status = MPFit.Solve(ForwardModels.GaussFunc, 10, 4, p, pars, null, v, ref result);

            Debug.Print("*** TestGaussFit status = {0}\n", status);
            PrintResult(p, pactual, result);

            return 0;
        }


        /* Test harness routine, which contains test gaussian-peak data 

           Example of fixing two parameter

           Commented example of how to put boundary constraints
        */
        private static int TestGaussFix()
        {
            double[] x = {-1.7237128E+00,1.8712276E+00,-9.6608055E-01,
		-2.8394297E-01,1.3416969E+00,1.3757038E+00,
		-1.3703436E+00,4.2581975E-02,-1.4970151E-01,
		8.2065094E-01};
            double[] y = {-4.4494256E-02,8.7324673E-01,7.4443483E-01,
		4.7631559E+00,1.7187297E-01,1.1639182E-01,
		1.5646480E+00,5.2322268E+00,4.2543168E+00,
		6.2792623E-01};
            double[] ey = new double[10];
            double[] p = { 0.0, 1.0, 0.0, 0.1 };       /* Initial conditions */
            double[] pactual = { 0.0, 4.70, 0.0, 0.5 };/* Actual values used to make data*/
            //double[] perror = new double[4];			   /* Returned parameter errors */
            int i;
            int status;

            mp_result result = new mp_result(4);
            //result.xerror = perror;

            mp_par[] pars = new mp_par[4]/* Parameter constraints */
                                {
                                    new mp_par(){isFixed = 1},/* Fix parameters 0 and 2 */
                                    new mp_par(), 
                                    new mp_par(){isFixed = 1},
                                    new mp_par()
                                }; 

            /* How to put limits on a parameter.  In this case, parameter 3 is
               limited to be between -0.3 and +0.2.
            pars[3].limited[0] = 0;    
            pars[3].limited[1] = 1;
            pars[3].limits[0] = -0.3;
            pars[3].limits[1] = +0.2;
            */

            for (i = 0; i < 10; i++)
            {
                ey[i] = 0.5;
            }

            CustomUserVariable v = new CustomUserVariable() { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 4 parameters (2
               parameters fixed) */
            status = MPFit.Solve(ForwardModels.GaussFunc, 10, 4, p, pars, null, v, ref result);

            Debug.Print("*** TestGaussFix status = {0}\n", status);
            PrintResult(p, pactual, result);

            return 0;
        }

        /* Simple routine to print the fit results */
        private static void PrintResult(double[] x, double[] xact, mp_result result)
        {
            int i;

            if (x == null) return;

            Debug.Print("  CHI-SQUARE = {0}    ({1} DOF)\n",
             result.bestnorm, result.nfunc - result.nfree);
            Debug.Print("        NPAR = {0}\n", result.npar);
            Debug.Print("       NFREE = {0}\n", result.nfree);
            Debug.Print("     NPEGGED = {0}\n", result.npegged);
            Debug.Print("       NITER = {0}\n", result.niter);
            Debug.Print("        NFEV = {0}\n", result.nfev);
            Debug.Print("\n");
            if (xact != null)
            {
                for (i = 0; i < result.npar; i++)
                {
                    Debug.Print("  P[{0}] = {1} +/- {2}     (ACTUAL {3})\n",
                       i, x[i], result.xerror[i], xact[i]);
                }
            }
            else
            {
                for (i = 0; i < result.npar; i++)
                {
                    Debug.Print("  P[{0}] = {1} +/- {2}\n",
                       i, x[i], result.xerror[i]);
                }
            }
        }
    }

}
