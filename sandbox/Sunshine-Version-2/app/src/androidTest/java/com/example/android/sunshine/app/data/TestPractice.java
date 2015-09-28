package com.example.android.sunshine.app.data;

import android.test.AndroidTestCase;

public class TestPractice extends AndroidTestCase {
    /*
        This gets run before every test.
     */
    @Override
    protected void setUp() throws Exception {
        super.setUp();
    }

    public void testThatDemonstratesAssertions() throws Throwable {
        int a = 5;
        int b = 3;
        int c = 5;
        int d = 10;

        // If assertion fails, given string message would be printed out.
        // Equals asserts a = c
        // True and false asserts following condition is true or false
        assertEquals("X should be equal", a, c);
        assertTrue("Y should be true", d > a);
        assertFalse("Z should be false", a == b);

        // Fail, done if a certain code path should never be reached
        if (b > d) {
            fail("XX should never happen");
        }
    }

    /*
        This gets run after every test.
     */
    @Override
    protected void tearDown() throws Exception {
        super.tearDown();
    }
}
