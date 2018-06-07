package client._main;

import org.apache.log4j.PropertyConfigurator;

public class Main
{
    public static void main(String[] args)
    {
        PropertyConfigurator.configure(MainConfig.log4jConfigPath);
        
    }
}
