/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package bittest;

import java.io.*;

/**
 *
 * @author XUYC
 */
public class BitTest {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws Exception {
        // TODO code application logic here
        
        FileInputStream fs = null;
        try
        {
            String filename = "D:\\Work\\mobile\\130.bin";
            fs = new FileInputStream(filename);
            
            byte[] buffer = new byte[1024];
            int len;
            while((len = fs.read(buffer, 0, buffer.length)) > 0)
            {
                for(int k=0; k<len; k+=4)
                {
                    byte bit1 = buffer[k], bit2 = buffer[k+1];
                    int area = ((bit1 & 0xFF) << 8) | (bit2 & 0xFF);
                    if(area < 0) 
                    {
                        break;
                    }
                    
                    System.out.println(area);
                }
            }
        }
        catch (Exception ex)
        {
            System.out.print(ex);
        }
        finally
        {
            if(fs != null)
                fs.close();
        }
        
        byte a = (byte)130, b = 2;
        int c = ((a & 0xFF) << 8) | b;
        
        System.out.print(c);
    }
}
