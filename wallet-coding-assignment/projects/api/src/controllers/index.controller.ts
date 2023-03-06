import { NextFunction, Request, Response } from 'express';
import userService from '@services/users.service';

class IndexController {
  public userService = new userService();

  public index = (req: Request, res: Response, next: NextFunction): void => {
    try {
      res.sendStatus(200);
    } catch (error) {
      next(error);
    }
  };

  public healthcheck = async (req: Request, res: Response, next: NextFunction): Promise<void> => {
    try {
      const numUsers = (await this.userService.findAllUser()).length;
      res.sendStatus(200);
    } catch (error) {
      next(error);
    }
  };
}

export default IndexController;
